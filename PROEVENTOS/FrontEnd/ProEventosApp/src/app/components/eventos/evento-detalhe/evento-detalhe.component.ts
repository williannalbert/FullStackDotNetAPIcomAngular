import { Component, OnInit, TemplateRef } from '@angular/core';
import { 
  AbstractControl, 
  FormArray, 
  FormBuilder, 
  FormControl, 
  FormGroup, 
  Validators 
} from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';

import { Evento } from '@app/models/Evento';
import { EventoService } from '@app/services/evento.service';
import { Lote } from '@app/models/Lote';
import { LoteService } from '@app/services/lote.service';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { Observable } from 'rxjs';
import { environment } from '@environment/environment';
@Component({
  selector: 'app-evento-detalhe',
  templateUrl: './evento-detalhe.component.html',
  styleUrls: ['./evento-detalhe.component.scss']
})
export class EventoDetalheComponent implements OnInit {

  modalRef: BsModalRef;
  eventoId: number;
  evento = {} as Evento;
  form: FormGroup;
  modoSalvar = 'post';
  loteAtual = {
    id: 0,
    nome: '',
    indice: 0
  }
  imagemURL = '/assets/cloud.jpg';
  file: File;

  get f():any{
    return this.form.controls;
  }

  get modoEditar():boolean{
    return this.modoSalvar === 'put';
  }

  lotes():FormArray{
    return this.form.get('lotes') as FormArray;
  }
  

  get bsConfig():any{
    return { 
      isAnimated: true,
      adaptivePosition: true,
      dateInputFormat: 'DD/MM/YYYY hh:mm a',
      containerClass: 'theme-default',
      showWeekNumbers: false
    }
  }

  get bsConfigLote():any{
    return { 
      isAnimated: true,
      adaptivePosition: true,
      dateInputFormat: 'DD/MM/YYYY',
      containerClass: 'theme-default',
      showWeekNumbers: false
    }
  }
  constructor(private fb: FormBuilder, 
    private localeService: BsLocaleService, 
    private activatedRouter: ActivatedRoute,
    private eventoService: EventoService,
    private loteService: LoteService,
    private spinner: NgxSpinnerService,
    private toastr: ToastrService,
    private router: Router,
    private modalService: BsModalService
  ){
    //this.localeService.use('pt-br')
  }

   public carregarEvento():void{
    this.eventoId = +this.activatedRouter.snapshot.paramMap.get('id');

    if(this.eventoId !== null && this.eventoId !== 0){
      
      this.spinner.show();
      
      this.modoSalvar = 'put';
      
      //this.eventoService.getEventoById(+this.eventoId).subscribe(
      this.eventoService.getEventoById(this.eventoId).subscribe(
        (evento: Evento) => {
          this.evento = {...evento},
          this.form.patchValue(this.evento)
          if(this.evento.imgUrl !== ''){
            this.imagemURL = environment.apiURL+'recursos/imagens/'+this.evento.imgUrl;
          }
          this.evento.lotes.forEach(lote => {
            this.lotes().push(this.criarLote(lote));
          })
          //this.carregarLote(); método feito para exemplo 
        },
        (errors: any) => {
          this.toastr.error('Erro ao carregar evento');
          console.log(errors);
        },
      ).add(() => this.spinner.hide());
    }
   }

  ngOnInit():void {
    this.validation();
    this.carregarEvento();
  }

  public validation(): void{
    this.form = this.fb.group({
      tema: [ '',
        [Validators.required, Validators.minLength(4), Validators.maxLength(50)]
      ],
      local: ['', Validators.required],
      dataEvento: ['', Validators.required],
      qtdPessoas: ['', 
        [Validators.required, Validators.max(120000)]
      ],
      imgUrl: [''] ,
      telefone: ['', Validators.required],
      email: ['', 
        [Validators.required, Validators.email]
      ],
      lotes: this.fb.array([])
    });
  }

  adicionarLote():void{
    this.lotes().push(
      this.criarLote({id: 0} as Lote)
    );
  }

  criarLote(lote: Lote):FormGroup{
    return this.fb.group({
      id: [lote.id],
      nome: [lote.nome, Validators.required],
      preco: [lote.preco, Validators.required],
      quantidade: [lote.quantidade, Validators.required],
      dataInicio: [lote.dataInicio],
      dataFim: [lote.dataFim],
    })
  }

  public carregarLote():void{
    this.loteService.getLoteByEventoId(this.eventoId).subscribe(
      (lotesRetorno: Lote[]) => {
        lotesRetorno.forEach(lote => {
          this.lotes().push(this.criarLote(lote));
        })
      },
      (error: any) => {
        this.toastr.error('Erro ao carregar lotes', 'Erro');
        console.log(error)
      }
    ).add(() => this.spinner.hide());
  }
  public resetForm(): void{
    this.form.reset();
  }

  public cssValidador(campo: FormControl | AbstractControl):any{
    return {'is-invalid':campo.errors && campo.touched}
  }

  public salvarEvento():void{
    
    if(this.form.valid){
      this.spinner.show();
      this.evento = (this.modoSalvar === 'post') ?
        {... this.form.value} //spread operator *funciona como um automapper*
      : {id: this.evento.id, ... this.form.value}
      };  

      this.eventoService[this.modoSalvar](this.evento).subscribe(
        (eventoRetorno:Evento) => {
          this.toastr.success('Evento salvo com sucesso', 'Sucesso');
          this.router.navigate([`eventos/detalhe/${eventoRetorno.id}`])
        },
        (error: any) => {
          console.log(error);
          this.spinner.hide();
          this.toastr.error('Ocorreu um erro ao salvar ebento', 'Erro');
        },
        () => this.spinner.hide()
      );  
    }

    public salvarLotes() : void{
      this.spinner.show();
      if(this.form.controls.lotes.valid){
        this.loteService.saveLote(this.eventoId, this.form.value.lotes)
        .subscribe(
          () => {
            this.toastr.success('Lotes salvos com sucesso', 'Sucesso');
            //this.lotes.reset()
          },
          (error:any) => {
            this.toastr.error('Erro ao salvar lotes', 'Erro');
            console.log(error)
          }
        ).add(() => this.spinner.hide());
      }
    }

    public removerLote(template: TemplateRef<any>, indice: number) : void{
      this.loteAtual.id = this.lotes().get(indice + '.id')?.value;
      this.loteAtual.nome = this.lotes().get(indice + '.nome')?.value;
      this.loteAtual.indice = indice
      
      this.modalRef = this.modalService.show(template, {class: 'modal-sm'})
    }

    public confirmDeleteLote():void{
      this.modalRef.hide();
      this.spinner.show();
      this.loteService.deleteLote(this.eventoId, this.loteAtual.id).subscribe(
        () => {
          this.toastr.success('Lote excluído com sucesso', 'Sucesso')
          this.lotes().removeAt(this.loteAtual.indice);
        },
        (error: any) => {
          this.toastr.error(`Erro ao excluir lote ${this.loteAtual.nome}`, 'Erro');
          console.log(error);
        }
      ).add(() => this.spinner.hide());
    }

    public declineDeleteLote(): void{
      this.modalRef.hide();
    }

    public mudarValorData(value: Date, indice: number, campo: string): void{
      this.lotes().value[indice][campo] = value;
    }

    public retornaTituloLote(valor: string) : string{
      return valor === null || valor ===  '' ? 'Nome do Lote' : valor;
    }

    onFileChange(ev: any): void{
      const reader = new FileReader();

      reader.onload = (event: any) => this.imagemURL = event.target.result;

      this.file = ev.target.files;
      reader.readAsDataURL(this.file[0])

      this.uploadImagem();
    }

    uploadImagem(): void{
      this.spinner.show();
      this.eventoService.postUpload(this.eventoId, this.file).subscribe(
        () => {
          //this.router.navigate([`eventos/detalhe/${this.eventoId}`]);
          this.carregarEvento();
          this.toastr.success('Imagem atualizada com sucesso', 'Sucesso')
        },
        (error: any) => {
          this.toastr.error('Erro ao atualizar imagem', 'Erroo');
          console.log(error);
        }
      ).add(() => this.spinner.hide());
    }
  }

