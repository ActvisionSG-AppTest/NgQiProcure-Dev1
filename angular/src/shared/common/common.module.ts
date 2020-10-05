import { CommonModule } from '@angular/common';
import { ModuleWithProviders, NgModule } from '@angular/core';
import { AppUrlService } from './nav/app-url.service';
import { AppUiCustomizationService } from './ui/app-ui-customization.service';
import { AppSessionService } from './session/app-session.service';
import { CookieConsentService } from './session/cookie-consent.service';
import { AppCustomSessionService } from './session/app-custom-session.service';

@NgModule({
    imports: [
        CommonModule
    ]
})
export class QiProcureDemoCommonModule {
    static forRoot(): ModuleWithProviders<CommonModule> {
        return {
            ngModule: CommonModule,
            providers: [
                AppUiCustomizationService,
                CookieConsentService,
                AppSessionService,
                AppCustomSessionService,               
                AppUrlService
            ]
        };
    }
}
