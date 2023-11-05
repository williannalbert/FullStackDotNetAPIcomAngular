import { Component, OnInit } from '@angular/core';
import { EventoService } from '../services/evento.service';
import { Evento } from '../models/Evento';

@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.scss'],
  //providers: [EventoService]
})
export class EventosComponent implements OnInit {

  public eventos: Evento[] = []; 
  public eventosFiltrados: Evento[] = []; 
  public widthImg: number = 150;
  public marginImg: number = 5;
  public mostrarImagem = true;
  private _filtroLista = '';

  public get filtroLista(): string{
    return this._filtroLista;
  }

  public set filtroLista(value: string){
    this._filtroLista = value;
    this.eventosFiltrados = this.filtroLista ? this.filtrarEventos(this.filtroLista) : this.eventos;
  }

  public filtrarEventos(filtrarPor: string) : Evento[] {
    filtrarPor = filtrarPor.toLocaleLowerCase();
    return this.eventos.filter(
      evento=> evento.tema.toLocaleLowerCase().indexOf(filtrarPor) !== -1
      || evento.local.toLocaleLowerCase().indexOf(filtrarPor) !== -1
    )
  }
  constructor(private eventoSevice: EventoService) { }

  public ngOnInit(): void {
    this.getEventos()
  }

  public exibirImagem(): void{
    this.mostrarImagem = !this.mostrarImagem
  }

  public getEventos(): void {

    this.eventoSevice.getEventos()
      .subscribe(
        (_eventos: Evento[]) => {
          this.eventos = _eventos
          this.eventosFiltrados = _eventos
        },
        error => console.log(error),
      );

  }
}
