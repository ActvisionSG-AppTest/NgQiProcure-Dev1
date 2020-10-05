import { Component, OnInit, Input } from "@angular/core";
import { ApprovalRequestsCustomServiceProxy, SysRefsCustomServiceProxy } from "@shared/service-proxies/service-custom-proxies";
import { AppConsts } from "@shared/AppConsts";
import { PrimengTableHelper } from "@shared/helpers/PrimengTableHelper";
import { finalize } from "rxjs/operators";

@Component({
    selector: "approval-list",
    templateUrl: "./approval-list.component.html",
    styleUrls: ["./approval-list.component.css"]
})
export class ApprovalListComponent implements OnInit {
    @Input("referenceId") referenceId: any;
    @Input("module") module: any;
    @Input("userName") userName: any;
    @Input("isUserAdmin") isUserAdmin: any;

    sysRefID: any;
    sysref: any;
    approvalMembers: any;
    isAdmin: boolean = false
    primengTableHelper: PrimengTableHelper;
    
    constructor( private _approvalRequestsCustomServiceProxy: ApprovalRequestsCustomServiceProxy,
        private _sysRefsCustomServiceProxy: SysRefsCustomServiceProxy) {}

    ngOnInit() {
        console.log(this.isUserAdmin)
        console.log(this.module)
        this.getApprovalMembers();
    }

    getApprovalMembers(){
        console.log("1", this.referenceId)
        console.log("2", this.module)
        this._sysRefsCustomServiceProxy.GetSysRefByRefType("Module",this.module).subscribe(result => {           
            var sysref = result.items;
            this.sysref = sysref;
            console.log("3",result.items)
             for (const sysref of result.items) {
                this.sysRefID = sysref.id;
                this._approvalRequestsCustomServiceProxy
                .getApprovalRequest(
                    this.referenceId,
                    this.sysref.id,
                    undefined,
                    undefined,
                    0,
                    999999
                )
                .subscribe(result => {
                    console.log("3")
                    this.approvalMembers = result.items;
                    console.log (this.approvalMembers)
                    for (const approvalMember of this.approvalMembers ) {
                        var profilePicture;
                        if (approvalMember.profilePicture == "") {
                            approvalMember.profilePicture =
                                AppConsts.appBaseUrl +
                                "/assets/common/images/default-profile-picture.png";
                        } else {
                            approvalMember.profilePicture =
                                "data:image/jpeg;base64," + approvalMember.profilePicture;
                        }
                        approvalMember.canApprove = false;
                        console.log("10",approvalMember.userName.toLowerCase())
                        if (approvalMember.userName.toLowerCase() == this.userName.toLowerCase() || this.isUserAdmin  == true)
                        {
                            approvalMember.canApprove = true;
                        } 
                    }

                });   
    
            }
        })
    }
    
    approveRequest(member) {
        if (member != null) {           
            console.log("1","approve") ;
        }
    }

    rejectRequest(member) {
        if (member != null) {           
            console.log("2","reject") ;
        }
    }

}
