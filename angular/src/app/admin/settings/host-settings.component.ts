import { Component, Injector, OnInit } from '@angular/core';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ComboboxItemDto, CommonLookupServiceProxy, SettingScopes, HostSettingsEditDto, HostSettingsServiceProxy, SendTestEmailInput  } from '@shared/service-proxies/service-proxies';
import { GetParamSettingForViewDto, CreateOrEditParamSettingDto, ParamSettingsCustomServiceProxy} from '@shared/service-proxies/service-custom-proxies';
import { finalize } from 'rxjs/operators';
@Component({
    templateUrl: './host-settings.component.html',
    animations: [appModuleAnimation()]
})
export class HostSettingsComponent extends AppComponentBase implements OnInit {
    loading = false;
    hostSettings: HostSettingsEditDto;
    editions: ComboboxItemDto[] = undefined;
    testEmailAddress: string = undefined;
    showTimezoneSelection = abp.clock.provider.supportsMultipleTimezone;
    defaultTimezoneScope: SettingScopes = SettingScopes.Application;

    usingDefaultTimeZone = false;
    initialTimeZone: string = undefined;

    paramsSetting: GetParamSettingForViewDto[];
    enabledSocialLoginSettings: string[];

    constructor(
        injector: Injector,
        private _hostSettingService: HostSettingsServiceProxy,
        private _commonLookupService: CommonLookupServiceProxy,
        private _paramSettingsServiceProxy: ParamSettingsCustomServiceProxy		
    ) {
        super(injector);
    }

    loadHostSettings(): void {
        const self = this;
        self._hostSettingService.getAllSettings()
            .subscribe(setting => {
                self.hostSettings = setting;
                self.initialTimeZone = setting.general.timezone;
                self.usingDefaultTimeZone = setting.general.timezoneForComparison === self.setting.get('Abp.Timing.TimeZone');
            });
    }

    loadEditions(): void {
        const self = this;
        self._commonLookupService.getEditionsForCombobox(false).subscribe((result) => {
            self.editions = result.items;

            const notAssignedEdition = new ComboboxItemDto();
            notAssignedEdition.value = null;
            notAssignedEdition.displayText = self.l('NotAssigned');

            self.editions.unshift(notAssignedEdition);
        });
    }

    init(): void {
        const self = this;
        self.testEmailAddress = self.appSession.user.emailAddress;
        self.showTimezoneSelection = abp.clock.provider.supportsMultipleTimezone;
        self.loadHostSettings();
        self.loadEditions();
        self.loadSocialLoginSettings();
    }

    ngOnInit(): void {
        const self = this;
        self.init();
    }

    sendTestEmail(): void {
        const self = this;
        const input = new SendTestEmailInput();
        input.emailAddress = self.testEmailAddress;
        self._hostSettingService.sendTestEmail(input).subscribe(result => {
            self.notify.info(self.l('TestEmailSentSuccessfully'));
        });
    }

    saveAll(): void {
        const self = this;

        if (!self.hostSettings.tenantManagement.defaultEditionId || self.hostSettings.tenantManagement.defaultEditionId.toString() === 'null') {
            self.hostSettings.tenantManagement.defaultEditionId = null;
        }

        self._hostSettingService.updateAllSettings(self.hostSettings).subscribe(result => {
            self.notify.info(self.l('SavedSuccessfully'));
			for (const paramSetting of this.paramsSetting){
                var param: CreateOrEditParamSettingDto = new CreateOrEditParamSettingDto
                param.id= paramSetting.paramSetting.id;
                param.name = paramSetting.paramSetting.name;
                if (paramSetting.paramSetting.boolValue!=null){
                    if  (paramSetting.paramSetting.boolValue == true){
                        param.value= 'true';
                    } else {
                        param.value= 'false';
                    }
                } else {
                    param.value = paramSetting.paramSetting.value;
                }
                param.description= paramSetting.paramSetting.description;
               
                this._paramSettingsServiceProxy.createOrEdit(param)
                .subscribe(() => {
                });    

            }
            if (abp.clock.provider.supportsMultipleTimezone && self.usingDefaultTimeZone && self.initialTimeZone !== self.hostSettings.general.timezone) {
                self.message.info(self.l('TimeZoneSettingChangedRefreshPageNotification')).then(() => {
                    window.location.reload();
                });
            }
        });
    }

	eventHandling(event:GetParamSettingForViewDto[]){
        this.paramsSetting = event;
    };

    loadSocialLoginSettings(): void {
        const self = this;
        this._hostSettingService.getEnabledSocialLoginSettings()
            .subscribe(setting => {
                self.enabledSocialLoginSettings = setting.enabledSocialLoginSettings;
            });
    }
}
