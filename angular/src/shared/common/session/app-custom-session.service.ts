import { AbpMultiTenancyService } from 'abp-ng2-module';
import { Injectable } from '@angular/core';
import { GetCurrentLoginInformationsOutput, SessionServiceProxy, TenantLoginInfoDto, UserLoginInfoDto, UiCustomizationSettingsDto, RoleServiceProxy, UserServiceProxy } from '@shared/service-proxies/service-proxies';
import { CommonQueryServiceProxy}  from '@shared/service-proxies/service-custom-proxies';
import { finalize } from 'rxjs/operators';

@Injectable()
export class AppCustomSessionService {

    private _user: UserLoginInfoDto;
    private _isUserAdmin: boolean;

    constructor(
        private _sessionService: SessionServiceProxy,
        private _abpMultiTenancyService: AbpMultiTenancyService,
        private _commonQueryService: CommonQueryServiceProxy,
        private _userServiceProxy: UserServiceProxy,
        ) {
    }

    get user(): UserLoginInfoDto {
        return this._user;
    }
   

    get isUserAdmin(): boolean {
       return this._isUserAdmin;
    }

    init(): Promise<UiCustomizationSettingsDto> {
        return new Promise<UiCustomizationSettingsDto>((resolve, reject) => {
            this._sessionService.getCurrentLoginInformations().toPromise().then((result: GetCurrentLoginInformationsOutput) => {              
                this._user = result.user;

                if (this._user != null) {
                    console.log('userName',this._user.userName);
                    this._commonQueryService.getIsUserAdmin(undefined,this._user.userName).toPromise().then(result => {
                        if (result.statusMessage.toLowerCase() == "success"){
                            this._isUserAdmin = result.status;
                        }
                    });                                       
                }

                resolve(result.theme);
            }, (err) => {
                reject(err);
            });

          

        });
    }

}
