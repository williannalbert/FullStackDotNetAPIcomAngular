import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.scss']
})
export class EventosComponent implements OnInit {

  public eventos: any = []; 
  public eventosFiltrados: any = []; 
  widthImg: number = 150;
  marginImg: number = 5;
  mostrarImagem = true;
  private _filtroLista = '';

  public get filtroLista(): string{
    return this._filtroLista;
  }

  public set filtroLista(value: string){
    this._filtroLista = value;
    this.eventosFiltrados = this.filtroLista ? this.filtrarEventos(this.filtroLista) : this.eventos;
  }

  filtrarEventos(filtrarPor: string) : any {
    filtrarPor = filtrarPor.toLocaleLowerCase();
    return this.eventos.filter(
      (evento : any) => evento.tema.toLocaleLowerCase().indexOf(filtrarPor) !== -1
      || evento.local.toLocaleLowerCase().indexOf(filtrarPor) !== -1
    )
  }
  constructor(private http:HttpClient) { }

  ngOnInit(): void {
    this.getEventos()
  }

  exibirImagem(){
    this.mostrarImagem = !this.mostrarImagem
  }

  public getEventos(): void {

    this.http.get('https://localhost:7120/api/evento')
      .subscribe(
        response => {
          this.eventos = response
          this.eventosFiltrados = response
        },
        error => console.log(error),
      );

  }
}
