import {
    Component,
    ViewChild,
    Injector,
    Output,
    EventEmitter,
    OnInit,
    ViewEncapsulation
} from "@angular/core";
import { ModalDirective } from "ngx-bootstrap/modal";
import { finalize } from "rxjs/operators";
import {
    RoleServiceProxy,
    UserServiceProxy,
    UserLinkServiceProxy,
} from "@shared/service-proxies/service-proxies";
import {
    TeamsCustomServiceProxy,
    CreateOrEditTeamDto,
    CreateOrEditTeamMemberDto,
} from "@shared/service-proxies/service-custom-proxies";
import { AppComponentBase } from "@shared/common/app-component-base";
import { TabDirective } from "ngx-bootstrap/tabs";
import { CodeHighlighterModule } from "primeng/codehighlighter";
import { TreeNode } from "primeng/api";

import * as moment from "moment";
import { TeamSysStatusLookupTableModalComponent } from "./team-sysStatus-lookup-table-modal.component";
import {
    UserCustomServiceProxy,
    SysRefsCustomServiceProxy,
    TeamMembersCustomServiceProxy,
    CustomTreeNode,
    ParamSettingsCustomServiceProxy,
    ApprovalsCustomServiceProxy,
    SysStatusesCustomServiceProxy,
    StatusList,
    SubmitNewTeamForApprovalInput,
    ApprovalRequestsCustomServiceProxy
} from "@shared/service-proxies/service-custom-proxies";
import { AppConsts } from "@shared/AppConsts";
import { TeamReferenceTypeLookupTableModalComponent } from "./team-referenceType-lookup-table-modal.component";
import { AdditionalData } from "./../../../../shared/service-proxies/service-proxies";
import { stat } from "fs";

@Component({
    selector: "createOrEditTeamModal",
    templateUrl: "./create-or-edit-team-modal.component.html",
    styleUrls: ["./create-or-edit-team-modal.component.css"]
})
export class CreateOrEditTeamModalComponent extends AppComponentBase
    implements OnInit {
    @ViewChild("createOrEditModal", { static: true }) modal: ModalDirective;
    @ViewChild("teamSysStatusLookupTableModal", { static: true })
    teamSysStatusLookupTableModal: TeamSysStatusLookupTableModalComponent;
    @ViewChild("teamReferenceTypeLookupTableModal", { static: true })
    teamReferenceTypeLookupTableModal: TeamReferenceTypeLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    module: string = "Team";
    activeTab: string;
    team: CreateOrEditTeamDto = new CreateOrEditTeamDto();
    teamMemberDetail: CreateOrEditTeamMemberDto = new CreateOrEditTeamMemberDto();

    sysStatusName = "";
    users: any[];
    teamMembers: any[] = [];
    userList: any[];
    teamMembersList: any[];
    teamRoles: any[] = [];
    teamRolesList: any[];

    otherTeamMembers: any;
    referenceTypeName = "";

    mainTeamChart: CustomTreeNode[];
    selectedNode: CustomTreeNode[];
    childrenTeamList: any;
    mainNodes = [];

    isTeamApprovalRequired: boolean = false;
    teamApproval: SubmitNewTeamForApprovalInput = new SubmitNewTeamForApprovalInput();

    status: StatusList = new StatusList();
    
    referenceId: number;
    sysRefId: number;

    userName: any;
    isUserAdmin: boolean = false;
    
    constructor(
        injector: Injector,
        private _teamsServiceProxy: TeamsCustomServiceProxy,
        private _userCustomServiceProxy: UserCustomServiceProxy,
        private _teamMembersCustomServiceProxy: TeamMembersCustomServiceProxy,
        private _sysRefsCustomServiceProxy: SysRefsCustomServiceProxy,
        private _paramCustomSettingsServiceProxy: ParamSettingsCustomServiceProxy,
        private _approvalsCustomServiceProxy: ApprovalsCustomServiceProxy,
        private _sysStatusesCustomServiceProxy: SysStatusesCustomServiceProxy,        
        private _userLinkServiceProxy: UserLinkServiceProxy,
        private _approvalRequestsCustomServiceProxy: ApprovalRequestsCustomServiceProxy,

    ) {
        super(injector);
    }

    ngOnInit() {
        this.users = [];
        this.teamMembers = [];
    }

    show(teamId?: number): void {

        this.getStatuses("Approval");
        if (!teamId) {
            this.team = new CreateOrEditTeamDto();
            this.team.id = teamId;
            this.referenceId = teamId;
            this.sysStatusName = "";            

            this.active = true;
            this.modal.show();
        } else {
            this.primengTableHelper.showLoadingIndicator();
            this._teamsServiceProxy.getTeamForEdit(teamId).subscribe(result => {
                this.team = result.team;
                this.sysStatusName = result.sysStatusName;
                this.referenceTypeName = result.referenceTypeName;
                this.active = true;
                this.populateAllUsers();
                this.populateTeamMembers();
                this.populateTeamRoles();

                this.displayTeamChart(undefined);
                this.IsNewTeamApprovalRequired();
                              
                this.modal.show();
            });
            this.primengTableHelper.hideLoadingIndicator();
        }
        this.userName = this.appSession.user.userName;
        this.isUserAdmin = this.appCustomSession.isUserAdmin;

    }

    populateAllUsers() {
        this._userCustomServiceProxy
            .getUserTeamProfiles(undefined, this.team.id, false, "")
            .pipe(
                finalize(() => this.primengTableHelper.hideLoadingIndicator())
            )
            .subscribe(result => {
                this.userList = result.items;
                if (this.userList) {
                    this.users = this.displayUsers();
                }
                //this.primengTableHelper.totalRecordsCount = result.totalCount;
            });
    }

    populateTeamMembers() {
        this._userCustomServiceProxy
            .getUserTeamProfiles(undefined, this.team.id, true, "")
            .pipe(
                finalize(() => this.primengTableHelper.hideLoadingIndicator())
            )
            .subscribe(result => {
                this.teamMembersList = result.items;
                if (this.teamMembersList) {
                    this.teamMembers = this.displayTeamMembers();
                }
            });
    }

    save(): void {
        this.saving = true;
        this.team.sysStatusId = this.status.New;
        this._teamsServiceProxy
            .createOrEdit(this.team)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe(() => {
                this.notify.info(this.l("SavedSuccessfully"));
                this.close();
                this.modalSave.emit(null);
            });
    }

    onNodeSelect(event) {
        //   this.messageService.add({severity: 'success', summary: 'Node Selected', detail: event.node.label});
    }

    updateTeamMembers() {
        this.message.confirm("", this.l("AreYouSure"), isConfirmed => {
            if (isConfirmed) {
                var currentTeamMembersList = [];
                //--process Team Members
                //--check if existing team not in the selected list then delete
                this._userCustomServiceProxy
                    .getUserTeamProfiles(undefined, this.team.id, true, "")
                    .pipe(
                        finalize(() =>
                            this.primengTableHelper.hideLoadingIndicator()
                        )
                    )
                    .subscribe(result => {
                        currentTeamMembersList = result.items;
                        if (currentTeamMembersList) {
                            for (const currentMember of currentTeamMembersList) {
                                var found = false;
                                for (const newMember of this.teamMembers) {
                                    if (currentMember.id == newMember.userId) {
                                        found = true;
                                    }
                                }
                                if (!found) {
                                    this._teamMembersCustomServiceProxy
                                        .delete(currentMember.teamMemberId)
                                        .subscribe(() => {});
                                }
                            }
                        }
                    });
                for (const member of this.teamMembers) {
                    var teamMember = new CreateOrEditTeamMemberDto();
                    teamMember.teamId = this.team.id;
                    teamMember.userId = member.userId;
                    this._teamMembersCustomServiceProxy
                        .createOrEdit(teamMember)
                        .pipe(
                            finalize(() => {
                                this.saving = false;
                            })
                        )
                        .subscribe(() => {
                            this.notify.info(this.l("UpdatedSuccessfully"));
                            this.populateTeamRoles();
                            this.teamRoles = this.displayTeamRoles();
                        });
                }
            }
        });
    }

    updateMemberDetail(member) {
        if (member.userId) {
            var indexMember = this.teamMembers.findIndex(
                m => m.userId == member.userId
            );
            var teamMember = this.teamMembers[indexMember];
            this._teamMembersCustomServiceProxy
                .getTeamMemberForEdit(teamMember.teamMemberId)
                .subscribe(result => {
                    this.teamMemberDetail = result.teamMember;
                    this.teamMemberDetail.sysRefId = member.teamRole;
                    this.teamMemberDetail.reportingTeamMemberId =
                        member.reportingTo || 0;

                    this._teamMembersCustomServiceProxy
                        .createOrEdit(this.teamMemberDetail)
                        .pipe(
                            finalize(() => {
                                this.saving = false;
                            })
                        )
                        .subscribe(() => {
                            this.notify.info(this.l("SavedSuccessfully"));

                            this.displayTeamChart(undefined);
                        });
                });
        }
    }
    openSelectSysStatusModal() {
        this.teamSysStatusLookupTableModal.id = this.team.sysStatusId;
        this.teamSysStatusLookupTableModal.displayName = this.sysStatusName;
        this.teamSysStatusLookupTableModal.show();
    }

    setSysStatusIdNull() {
        this.team.sysStatusId = null;
        this.sysStatusName = "";
    }

    getNewSysStatusId() {
        this.team.sysStatusId = this.teamSysStatusLookupTableModal.id;
        this.sysStatusName = this.teamSysStatusLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    onSelectTab(data: TabDirective): void {
        this.activeTab = data.heading;

        if (this.activeTab == "Team Info") {
            this.populateTeamMembers();
            this.teamMembers = this.displayTeamMembers();
        }

        if (this.activeTab == "Members") {
            this.populateAllUsers();
            this.users = this.displayUsers();
            this.populateTeamMembers();
            this.teamMembers = this.displayTeamMembers();
        }

        if (this.activeTab == "Member Details") {
            this.populateTeamRoles();
            this.teamRoles = this.displayTeamRoles();
            this.populateTeamMembers();
            this.users = this.displayUsers();
            this.populateTeamMembers();
            this.teamMembers = this.displayTeamMembers();
        }

        if (this.activeTab == "Team Chart") {
            this.displayTeamChart(undefined);
        }
    }

    displayUsers() {
        const users = [];
        for (const user of this.userList) {
            var profilePicture;
            if (user.profilePicture == "") {
                profilePicture =
                    AppConsts.appBaseUrl +
                    "/assets/common/images/default-profile-picture.png";
            } else {
                profilePicture =
                    "data:image/jpeg;base64," + user.profilePicture;
            }
            users.push({
                teamMemberId: user.teamMemberId,
                userId: user.id,
                name: user.name,
                surname: user.surname,
                userName: user.userName,
                emailAddress: user.emailAddress,
                profilePicture: profilePicture
            });
        }
        return users;
    }

    displayTeamMembers() {
        const members = [];
        for (const member of this.teamMembersList) {
            var profilePicture;
            if (member.profilePicture == "") {
                profilePicture =
                    AppConsts.appBaseUrl +
                    "/assets/common/images/default-profile-picture.png";
            } else {
                profilePicture =
                    "data:image/jpeg;base64," + member.profilePicture;
            }
            members.push({
                label: member.name + " " + member.surname,
                value: member.teamMemberId,
                teamMemberId: member.teamMemberId,
                userId: member.id,
                name: member.name,
                surname: member.surname,
                userName: member.userName,
                emailAddress: member.emailAddress,
                profilePicture: profilePicture,
                reportingTo: member.reportingTo,
                teamRole: member.selectedRole
            });
        }
        return members;
    }

    populateTeamRoles() {
        this._sysRefsCustomServiceProxy
            .getCustomAll(
                "",
                "",
                "",
                "",
                this.team.referenceTypeId,
                undefined,
                0,
                999999
            )
            .subscribe(result => {
                this.teamRolesList = result.items;
                if (this.teamRolesList) {
                    this.teamRoles = this.displayTeamRoles();
                }
            });
    }

    displayTeamRoles() {
        const teamRoles = [];
        for (const role of this.teamRolesList) {
            teamRoles.push({
                label: role.sysRef.refCode,
                value: role.sysRef.id,
                roleName: role.sysRef.refCode,
                refType: role.referenceTypeName,
                sysRefId: role.sysRef.id
            });
        }
        return teamRoles;
    }

    submitforApproval(team) {
        if (this.team.id != null) {
            this.teamApproval.teamId = this.team.id;
            if (this.sysStatusName.toLowerCase() == "new") {
                this.message.confirm("", this.l("AreYouSure"), isConfirmed => {
                    if (isConfirmed) {
                        //read team approver and insert into QP_ApprovalRequests
                        this.teamApproval.sysStatusId = this.status.Pending;
                        this._approvalsCustomServiceProxy
                            .submitNewTeamForApproval(this.teamApproval)
                            .pipe(
                                finalize(() => {
                                    this.saving = false;
                                })
                            )
                            .subscribe(result => {
                                if (result.status == true){
                                    this.notify.info(this.l("ApprovalSubmittedSuccessfully"));                                   
                                        
                                    this._teamsServiceProxy.getTeamForEdit(this.team.id).subscribe(result => {
                                        this.team = result.team;
                                        this.sysStatusName = result.sysStatusName;
                                        this.referenceTypeName = result.referenceTypeName;                        
                                    });
                                    //this.close();
                                    //this.modalSave.emit(null);
                                    
                                    
                                    
                                    // // get approvals and display
                                    // this._approvalRequestsCustomServiceProxy
                                    // .getApprovalRequest(
                                    //     this.team.id,
                                    //     undefined,
                                    //     undefined,
                                    //     undefined,
                                    //     0,
                                    //     999999
                                    // )
                                    // .subscribe(result => {
                                    //     console.log("approval requests", result);
                                    // });
                                } else {

                                }
                            });
                    }
                });
            }
        }
    }

    displayOtherMembers(disabled: boolean) {
        if (disabled) {
            event.stopPropagation();
        }
    }

    onSelectedReportingTo(member, event) {
        var indexMember = this.teamMembers.findIndex(
            m => m.userId == member.userId
        );
        if (member.value == event.value) {
            this.teamMembers[indexMember].reportingTo = "";
        }
    }

    openSelectReferenceTypeModal() {
        this.teamReferenceTypeLookupTableModal.id = this.team.referenceTypeId;
        this.teamReferenceTypeLookupTableModal.id = this.team.referenceTypeId;
        this.teamReferenceTypeLookupTableModal.displayName = this.referenceTypeName;
        this.teamReferenceTypeLookupTableModal.show();
    }

    setReferenceTypeIdNull() {
        this.team.referenceTypeId = null;
        this.referenceTypeName = "";
    }

    getNewReferenceTypeId() {
        this.team.referenceTypeId = this.teamReferenceTypeLookupTableModal.id;
        this.referenceTypeName = this.teamReferenceTypeLookupTableModal.displayName;
    }

    assignTeamChartChildrenSingle(member) {
        var members = [];
        if (member != null) {
            var ProfilePicture;
            if (member.profilePicture == "") {
                ProfilePicture =
                    AppConsts.appBaseUrl +
                    "/assets/common/images/default-profile-picture.png";
            } else {
                ProfilePicture =
                    "data:image/jpeg;base64," + member.profilePicture;
            }
            members.push({
                teamMemberId: member.teamMember.id,
                reportingTo: member.teamMember.reportingTeamMemberId,
                emailAddress: member.emailAddress,
                label: member.selectedRoleName,
                type: "person",
                styleClass: "ui-person",
                expanded: true,
                data: { name: member.fullName, avatar: ProfilePicture },
                children: []
            });
        }

        return members;
    }

    displayTeamChart(teamMemberId) {
        this.populateTeamChart(teamMemberId).then(result => {
            var node = [];

            for (const teamMember of result) {
                if (teamMember.teamMember.reportingTeamMemberId == 0) {
                    node = this.assignTeamChartChildrenSingle(teamMember);
                } else {
                    node = this.browseTeamNodes(
                        node,
                        teamMember,
                        teamMember.teamMember.reportingTeamMemberId
                    );
                }
            }

            this.mainTeamChart = node;
        });
    }

    populateTeamChart(reportingTeamMemberId): Promise<any> {
        return new Promise((resolve, reject) => {
            this._teamMembersCustomServiceProxy
                .getTeamMembers(
                    undefined,
                    undefined,
                    reportingTeamMemberId,
                    this.team.id,
                    "",
                    "",
                    "",
                    ""
                )
                .pipe(
                    finalize(() =>
                        this.primengTableHelper.hideLoadingIndicator()
                    )
                )
                .subscribe(result => {
                    resolve(result.items);
                });
        });
    }

    browseTeamNodes(parentNode, member, teamMemberId) {
        var found: boolean = false;

        if (parentNode != null) {
            for (let i = 0; i < parentNode.length; i++) {
                if (teamMemberId == parentNode[0].teamMemberId) {
                    var ProfilePicture;
                    if (member.profilePicture == "") {
                        ProfilePicture =
                            AppConsts.appBaseUrl +
                            "/assets/common/images/default-profile-picture.png";
                    } else {
                        ProfilePicture =
                            "data:image/jpeg;base64," + member.profilePicture;
                    }
                    parentNode[0].children.push({
                        teamMemberId: member.teamMember.id,
                        reportingTo: member.teamMember.reportingTeamMemberId,
                        emailAddress: member.emailAddress,
                        label: member.selectedRoleName,
                        type: "person",
                        styleClass: "ui-person",
                        expanded: true,
                        data: { name: member.fullName, avatar: ProfilePicture },
                        children: []
                    });
                    i = parentNode.length + 1;
                } else {
                    this.browseTeamNodes(
                        parentNode[0].children,
                        member,
                        teamMemberId
                    );
                    i = parentNode.length + 1;
                }
            }
        }

        return parentNode;
    }

    IsNewTeamApprovalRequired() {
        this._paramCustomSettingsServiceProxy
            .getAll(
                undefined,
                "IsNewTeamApprovalRequired",
                undefined,
                undefined,
                undefined,
                undefined,
                999999
            )
            .subscribe(result => {
                if (result.totalCount > 0) {
                    for (const paramSetting of result.items) {
                        this.isTeamApprovalRequired =
                            paramSetting.paramSetting.boolValue || false;
                    }
                }
            });
    }

    getStatuses(statusFor) {
        this._sysStatusesCustomServiceProxy
            .getCustomAll(
                undefined,
                undefined,
                undefined,
                undefined,
                undefined,
                undefined,
                statusFor,
                undefined,
                undefined,
                999999
            )
            .subscribe(result => {
                for (const status of result.items) {
                    if (status.sysStatus.name.toLowerCase().includes("new")) {this.status.New = status.sysStatus.id;}
                    if (status.sysStatus.name.toLowerCase().includes("required")) {this.status.Pending = status.sysStatus.id;}
                    if (status.sysStatus.name.toLowerCase().includes("pending")) {this.status.Pending = status.sysStatus.id;}
                    if (status.sysStatus.name.toLowerCase().includes("approve")) {this.status.Approved = status.sysStatus.id;}
                    if (status.sysStatus.name.toLowerCase().includes("reject")) {this.status.Rejected = status.sysStatus.id;}}
            });
    }
}
