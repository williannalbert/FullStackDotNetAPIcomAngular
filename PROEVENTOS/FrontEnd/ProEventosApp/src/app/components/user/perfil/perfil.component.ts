import { Component, OnInit } from '@angular/core';
import { UserUpdate } from '@app/models/Identity/UserUpdate';
import { AccountService } from '@app/services/account.service';
import { environment } from '@environment/environment';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-perfil',
  templateUrl: './perfil.component.html',
  styleUrls: ['./perfil.component.scss']
})
export class PerfilComponent implements OnInit {
  public usuario = {} as UserUpdate; 
  public imagemURL = '';
  public file: File;

  public get isPalestrante():boolean{
    return this.usuario.funcao === 'Palestrante';
  }

  constructor(
    private spinner: NgxSpinnerService,
    private toastr: ToastrService,
    private accountService: AccountService
  ) { }

  ngOnInit() {

  }

  public setFormValue(usuario: UserUpdate):void{
    this.usuario = usuario;
    if(this.usuario.imgUrl)
      this.imagemURL = environment.apiURL+`recursos/perfil/${this.usuario.imgUrl}`;
    else
    this.imagemURL ='./assets/img/perfil.png';

    console.log(this.usuario);
  }

  onFileChange(ev: any): void{
    const reader = new FileReader();

    reader.onload = (event: any) => this.imagemURL = event.target.result;

    this.file = ev.target.files;
    reader.readAsDataURL(this.file[0])

    this.uploadImagem();
  }


  private uploadImagem(): void{
    this.spinner.show();
    this.accountService.postUpload(this.file).subscribe(
      () => {
        //this.router.navigate([`eventos/detalhe/${this.eventoId}`]);
        this.toastr.success('Imagem atualizada com sucesso', 'Sucesso')
      },
      (error: any) => {
        this.toastr.error('Erro ao atualizar imagem', 'Erro');
        console.log(error);
      }
    ).add(() => this.spinner.hide());
  }
}
