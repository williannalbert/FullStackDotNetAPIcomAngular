import { Component, OnInit } from '@angular/core';
import { AbstractControlOptions, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ValidatorField } from '@app/helpers/ValidatorField';
import { UserUpdate } from '@app/models/Identity/UserUpdate';
import { AccountService } from '@app/services/account.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-perfil',
  templateUrl: './perfil.component.html',
  styleUrls: ['./perfil.component.scss']
})
export class PerfilComponent implements OnInit {
  userUpdate = {} as UserUpdate; 
  form : FormGroup;

  constructor(public fb: FormBuilder,
    public accountService: AccountService,
    private router: Router,
    private toaster: ToastrService,
    private spinner: NgxSpinnerService) { }

  get f(): any{
    return this.form.controls;
  }

  ngOnInit() {
    this.validation();
    this.carregarUsuario();
  }

   private carregarUsuario(): void{
    this.spinner.show();
    this.accountService.getUser().subscribe(
      (userRetorno: UserUpdate) => {
        this.userUpdate = userRetorno;
        this.form.patchValue(this.userUpdate);
        this.toaster.success('Usuário carregado com sucesso', 'Sucesso');
      },
      (error) => {
        console.error(error)
        this.toaster.error('Usuário não carregado carregado', 'Erro');
        this.router.navigate(['/dashboard']);
      }
    ).add(this.spinner.hide());
   }
  private validation():void{

    const formOptions: AbstractControlOptions = {
      validators: ValidatorField.MustMatch('password', 'confirmaPassword')
    }

    this.form = this.fb.group({
      userName: [''],
      titulo: ['NaoInformado', Validators.required],
      primeiroNome: ['', Validators.required],
      ultimoNome: ['', Validators.required],
      email: ['', 
        [Validators.required, , Validators.email]
      ],
      phoneNumber: ['', Validators.required],
      funcao: ['NaoInformado', Validators.required],
      descricao: ['', Validators.required],
      password: ['', 
        [Validators.required, Validators.minLength(6)]
      ],
      confirmaPassword: ['', Validators.required],
    }, formOptions);
  }

  public resetForm():void{
    this.form.reset();
  }

  onSubmit():void{
    this.atualizarUsuario();
  }

  public atualizarUsuario(): void{
    this.userUpdate = { ... this.form.value}
    this.spinner.show();

    this.accountService.updateUser(this.userUpdate).subscribe(
      () => {this.toaster.success('Usuário atualizado com sucesso', 'Sucesso')},
      (error) => {
        this.toaster.error(error.error);
        console.error(error)
      },
    ).add(() => this.spinner.hide())
  }
}
