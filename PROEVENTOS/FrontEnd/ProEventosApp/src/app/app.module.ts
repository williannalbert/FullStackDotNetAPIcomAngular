import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http'
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { CollapseModule } from 'ngx-bootstrap/collapse';
import { TooltipModule } from 'ngx-bootstrap/tooltip';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { ModalModule } from 'ngx-bootstrap/modal';

import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { defineLocale } from 'ngx-bootstrap/chronos';
import { ptBrLocale } from 'ngx-bootstrap/locale';
import { NgxCurrencyModule } from 'ngx-currency';

import { ToastrModule } from 'ngx-toastr';
import { NgxSpinnerModule } from 'ngx-spinner';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { EventosComponent } from './components/eventos/eventos.component';
import { PalestrantesComponent } from './components/palestrantes/palestrantes.component';
import { ContatosComponent } from './components/contatos/contatos.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { PerfilComponent } from './components/user/perfil/perfil.component';
import { EventoDetalheComponent } from './components/eventos/evento-detalhe/evento-detalhe.component';
import { EventoListaComponent } from './components/eventos/evento-lista/evento-lista.component';
import { UserComponent } from './components/user/user.component';
import { LoginComponent } from './components/user/login/login.component';
import { RegistrationComponent } from './components/user/registration/registration.component';

import { JwtInterceptor } from './interceptors/jwt.interceptor';

import { TituloComponent } from './shared/titulo/titulo.component';
import { NavComponent } from './shared/nav/nav.component';

import { EventoService } from './services/evento.service';
import { LoteService } from './services/lote.service';
import { AccountService } from './services/account.service';

import { DateTimeFormatPipe } from './helpers/DateTimeFormat.pipe';
import { HomeComponent } from './components/home/home.component';

defineLocale('pt-br', ptBrLocale);

@NgModule({
  declarations: [		
    AppComponent,
    EventosComponent,
    PalestrantesComponent,
    ContatosComponent,
    DashboardComponent,
    PerfilComponent,
    TituloComponent,
    NavComponent,
    DateTimeFormatPipe,
    EventoDetalheComponent,
    EventoListaComponent,
    UserComponent,
    LoginComponent,
    RegistrationComponent,
    HomeComponent
   ],
  imports: [
    BrowserModule,
    FormsModule,
    ReactiveFormsModule,
    AppRoutingModule,
    HttpClientModule,
    BrowserAnimationsModule,
    CollapseModule.forRoot(),
    TooltipModule.forRoot(),
    BsDropdownModule.forRoot(),
    BsDatepickerModule.forRoot(),
    ModalModule.forRoot(),
    ToastrModule.forRoot({
      timeOut: 5000,
      positionClass: 'toast-bottom-right',
      preventDuplicates: true,
      progressBar: true
    }),
    NgxSpinnerModule,
    NgxCurrencyModule,
  ],
  providers: [
    AccountService,
    EventoService, 
    LoteService,
    {provide:HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true}
  ],
  bootstrap: [AppComponent],
  
})
export class AppModule { }
//schemas: [CUSTOM_ELEMENTS_SCHEMA],