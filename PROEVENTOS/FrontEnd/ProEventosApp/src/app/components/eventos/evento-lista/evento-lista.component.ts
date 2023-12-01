import { Component, OnInit, TemplateRef } from '@angular/core';
import { Router } from '@angular/router';

import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';

import { Evento } from '@app/models/Evento';
import { EventoService } from '@app/services/evento.service';
import { environment } from '@environment/environment';

@Component({
  selector: 'app-evento-lista',
  templateUrl: './evento-lista.component.html',
  styleUrls: ['./evento-lista.component.scss']
})
export class EventoListaComponent implements OnInit {

  modalRef = {} as BsModalRef;
  public eventos: Evento[] = []; 
  public eventosFiltrados: Evento[] = []; 
  public eventoId: number = 0;

  public widthImg = 150;
  public marginImg = 5;
  public mostrarImagem = true;
  private filtroListado = '';

  public get filtroLista(): string{
    return this.filtroListado;
  }

  public set filtroLista(value: string){
    this.filtroListado = value;
    this.eventosFiltrados = this.filtroLista ? this.filtrarEventos(this.filtroLista) : this.eventos;
  }

  public filtrarEventos(filtrarPor: string) : Evento[] {
    filtrarPor = filtrarPor.toLocaleLowerCase();
    return this.eventos.filter(
      evento=> evento.tema.toLocaleLowerCase().indexOf(filtrarPor) !== -1
      || evento.local.toLocaleLowerCase().indexOf(filtrarPor) !== -1
    );
  }
  constructor(private eventoSevice: EventoService,
    private modalService: BsModalService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
    private router: Router)
     { }

  public ngOnInit(): void {
    this.spinner.show();

    this.carregarEventos();
  }

  public exibirImagem(): void{
    this.mostrarImagem = !this.mostrarImagem
  }

  public carregarEventos(): void {
    this.eventoSevice.getEventos()
      .subscribe({
        next: (eventosResp: Evento[]) => {
          this.eventos = eventosResp
          this.eventosFiltrados = eventosResp
        },
        error:(error: any) => {
          this.spinner.hide();
          this.toastr.error('Erro ao carregar eventos', 'Erro')
        },
        complete: () => this.spinner.hide()
    });
  }

  openModal(event:any, template: TemplateRef<any>, eventoId: number): void {
    event.stopPropagation();
    this.eventoId = eventoId;
    this.modalRef = this.modalService.show(template, {class: 'modal-sm'});
  }
 
  confirm(): void {
    this.modalRef.hide();
    this.spinner.show();

    this.eventoSevice.deleteEvento(this.eventoId).subscribe(
      (result: any) => {
        if(result.message === 'Exclusão realizada com sucesso'){
          this.toastr.success('Exclusão realizada com sucesso', 'Sucesso');
          this.carregarEventos();
        }
      },
      (error: any) => {
        console.log(error)
        this.toastr.error(`Erro ao deletar evento de código ${this.eventoId}`, 'Erro');
      }
    ).add(() => this.spinner.hide());

    
  }
 
  decline(): void {
    this.modalRef.hide();
  }

  detalheEvento(id:number):void{
    this.router.navigate([`eventos/detalhe/${id}`]);
  }

  public retornaImagem(imagemURL:string):string{
    return (imagemURL !== '')
      ? `${environment.apiURL}recursos/imagens/${imagemURL}`
      : 'assets/img/notFound.png';
  }

}
