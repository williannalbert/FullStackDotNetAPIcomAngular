import { Component, OnInit } from '@angular/core';
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
@Component({
  selector: 'app-evento-detalhe',
  templateUrl: './evento-detalhe.component.html',
  styleUrls: ['./evento-detalhe.component.scss']
})
export class EventoDetalheComponent implements OnInit {

  eventoId: number;
  evento = {} as Evento;
  form: FormGroup;
  modoSalvar = 'post';

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
  constructor(private fb: FormBuilder, 
    private localeService: BsLocaleService, 
    private activatedRouter: ActivatedRoute,
    private eventoService: EventoService,
    private loteService: LoteService,
    private spinner: NgxSpinnerService,
    private toastr: ToastrService,
    private router: Router
  ){
    //this.localeService.use('pt-br')
  }

   public carregarEvento():void{
    this.eventoId = +this.activatedRouter.snapshot.paramMap.get('id');

    if(this.eventoId !== null || this.eventoId === 0){
      
      this.spinner.show();
      
      this.modoSalvar = 'put';
      
      //this.eventoService.getEventoById(+this.eventoId).subscribe(
      this.eventoService.getEventoById(this.eventoId).subscribe(
        (evento: Evento) => {
          this.evento = {...evento},
          this.form.patchValue(this.evento)
        },
        (errors: any) => {
          this.spinner.hide();
          this.toastr.error('Erro ao carregar evento');
          console.log(errors);
        },
        () => this.spinner.hide(),
      )
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
      imgUrl: ['', Validators.required] ,
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

  public resetForm(): void{
    this.form.reset();
  }

  public cssValidador(campo: FormControl | AbstractControl):any{
    return {'is-invalid':campo.errors && campo.touched}
  }

  public salvarEvento():void{
    this.spinner.show();

    if(this.form.valid){
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
  }

