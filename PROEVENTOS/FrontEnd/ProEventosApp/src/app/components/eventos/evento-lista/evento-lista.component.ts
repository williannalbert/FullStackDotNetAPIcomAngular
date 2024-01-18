import { Component, OnInit, TemplateRef } from '@angular/core';
import { Router } from '@angular/router';

import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';

import { Evento } from '@app/models/Evento';
import { EventoService } from '@app/services/evento.service';
import { environment } from '@environment/environment';
import { PaginatedResult, Pagination } from '@app/models/Pagination';
import { Subject } from 'rxjs';
import { debounceTime } from 'rxjs/operators';

@Component({
  selector: 'app-evento-lista',
  templateUrl: './evento-lista.component.html',
  styleUrls: ['./evento-lista.component.scss']
})
export class EventoListaComponent implements OnInit {

  modalRef = {} as BsModalRef;
  public eventos: Evento[] = []; 
  public eventoId: number = 0;
  public pagination = {} as Pagination;

  public widthImg = 150;
  public marginImg = 5;
  public mostrarImagem = true;

  termoBuscaChanged: Subject<string> = new Subject<string>();

  public filtrarEventos(evt:any) :void {
    if(this.termoBuscaChanged.observers.length === 0){
      this.termoBuscaChanged.pipe(debounceTime(1000)).subscribe(
        filtrarPor => {
          this.spinner.show();
          this.eventoSevice.getEventos(
            this.pagination.currentPage,
            this.pagination.itemsPerPage,
            filtrarPor
          )
          .subscribe((paginatedResult: PaginatedResult<Evento[]>) => {
            this.eventos = paginatedResult.result;
            this.pagination = paginatedResult.pagination
          },
          (error: any) => {
            this.spinner.hide();
            this.toastr.error('Erro ao carregar eventos', 'Erro')
          })
          .add(() => this.spinner.hide());
        }
      )
    }
    this.termoBuscaChanged.next(evt.value);
  }
  constructor(private eventoSevice: EventoService,
    private modalService: BsModalService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
    private router: Router)
     { }

  public ngOnInit(): void {
    this.pagination = {currentPage: 1, itemsPerPage: 4, totalItems: 1} as Pagination;
    
    this.carregarEventos();
  }

  public exibirImagem(): void{
    this.mostrarImagem = !this.mostrarImagem
  }

  public carregarEventos(): void {
    this.spinner.show();

    this.eventoSevice.getEventos(this.pagination.currentPage, this.pagination.itemsPerPage)
      .subscribe(
        (paginatedResult: PaginatedResult<Evento[]>) => {
          this.eventos = paginatedResult.result;
          this.pagination = paginatedResult.pagination
        },
        (error: any) => {
          this.spinner.hide();
          this.toastr.error('Erro ao carregar eventos', 'Erro')
        },
    ).add(() => this.spinner.hide());
  }

  openModal(event:any, template: TemplateRef<any>, eventoId: number): void {
    event.stopPropagation();
    this.eventoId = eventoId;
    this.modalRef = this.modalService.show(template, {class: 'modal-sm'});
  }

  public pageChanged(event):void{
    this,this.pagination.currentPage = event.page;
    this.carregarEventos();
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
