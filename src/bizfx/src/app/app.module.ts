import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClient, HTTP_INTERCEPTORS } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

/* SPEAK */
import { ScActionBarModule } from '@speak/ng-bcl/action-bar';
import { ScGlobalHeaderModule } from '@speak/ng-bcl/global-header';
import { ScGlobalLogoModule } from '@speak/ng-bcl/global-logo';
import { ScApplicationHeaderModule } from '@speak/ng-bcl/application-header';
import { ScPageModule } from '@speak/ng-bcl/page';
import { ScActionControlModule } from '@speak/ng-bcl/action-control';
import { ScDialogModule } from '@speak/ng-bcl/dialog';
import { ScDropdownModule } from '@speak/ng-bcl/dropdown';
import { ScExpanderModule } from '@speak/ng-bcl/expander';
import { ScProgressIndicatorPanelModule } from '@speak/ng-bcl/progress-indicator-panel';
import { ScTableModule } from '@speak/ng-bcl/table';

/* Third Party Modules */
import { CookieModule } from 'ngx-cookie';
import { CKEditorModule } from 'ng2-ckeditor';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { TagInputModule } from 'ngx-chips';
import { TranslateModule, TranslateLoader } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';

/* Feature Modules */
import { ScBizFxModule, ScHttpXsrfInterceptor } from '@sitecore/bizfx';

/* Routing Module */
import { AppRoutingModule } from './app-routing.module';

/* App Root */
import { AppComponent } from './app.component';
import {
  ScBizFxActionComponent,
  ScBizFxActionBarComponent,
  ScBizFxFlatViewComponent,
  ScBizFxItemViewComponent,
  ScBizFxListPoliciesComponent,
  ScBizFxListViewComponent,
  ScBizFxSearchViewComponent,
  ScBizFxTableViewComponent,
  ScBizFxViewPropertyByTypeComponent,
  ScBizFxViewPropertyByUiComponent,
  ScBizFxViewPropertyTagsComponent,
  ScBizFxActionGridComponent,
  ScBizFxActionPropertyDateTimeComponent,
  ScBizFxActionPropertyTagsComponent,
  ScBizFxActionPropertyComponent,
  ScBizFxAutocompleteComponent,
  ScBizFxBraintreeComponent,
  ScBizFxMediaPickerComponent,
  ScBizFxViewComponent,
  ScBizfxFiledownloadComponent,
  ScBizfxFileuploadComponent,
  ScBizfxLongrunningcommandComponent,
} from './components';

/**
 * @ignore
 */
// AoT requires an exported function for factories
export function HttpLoaderFactory(http: HttpClient) {
  return new TranslateHttpLoader(http);
}

import { registerLocaleData } from '@angular/common';

/* Locales */
import localeFr from '@angular/common/locales/fr';
import localeFrExtra from '@angular/common/locales/extra/fr';
import localeJa from '@angular/common/locales/ja';
import localeJaExtra from '@angular/common/locales/extra/ja';
import localeDe from '@angular/common/locales/de';
import localeDeExtra from '@angular/common/locales/extra/de';

registerLocaleData(localeFr, 'fr-FR', localeFrExtra);
registerLocaleData(localeJa, 'Ja-JP', localeJaExtra);
registerLocaleData(localeDe, 'de-DE', localeDeExtra);

/**
 * BizFxLib `Module`.
 */
@NgModule({
  imports: [
    BrowserModule,
    BrowserAnimationsModule,

    CookieModule.forRoot(),
    CKEditorModule,
    TagInputModule,
    NgbModule,
    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useFactory: HttpLoaderFactory,
        deps: [HttpClient]
      }
    }),
    ScActionControlModule,
    ScActionBarModule,
    ScDialogModule,
    ScGlobalHeaderModule,
    ScGlobalLogoModule,
    ScDropdownModule,
    ScApplicationHeaderModule,
    ScExpanderModule,
    ScPageModule,
    ScProgressIndicatorPanelModule,
    ScTableModule,

    ScBizFxModule,

    AppRoutingModule
  ],
  declarations: [
    AppComponent,
    ScBizFxActionComponent,
    ScBizFxActionBarComponent,
    ScBizFxFlatViewComponent,
    ScBizFxItemViewComponent,
    ScBizFxListPoliciesComponent,
    ScBizFxListViewComponent,
    ScBizFxSearchViewComponent,
    ScBizFxTableViewComponent,
    ScBizFxViewPropertyByTypeComponent,
    ScBizFxViewPropertyByUiComponent,
    ScBizFxViewPropertyTagsComponent,

    ScBizFxActionGridComponent,
    ScBizFxActionPropertyDateTimeComponent,
    ScBizFxActionPropertyTagsComponent,
    ScBizFxActionPropertyComponent,
    ScBizFxAutocompleteComponent,
    ScBizFxBraintreeComponent,
    ScBizFxMediaPickerComponent,
    ScBizFxViewComponent,
    ScBizfxFiledownloadComponent,
    ScBizfxFileuploadComponent,
    ScBizfxLongrunningcommandComponent,
  ],
  providers: [
    {provide: HTTP_INTERCEPTORS, useClass: ScHttpXsrfInterceptor, multi: true}
  ],
  bootstrap: [AppComponent]
})

export class AppModule { }
