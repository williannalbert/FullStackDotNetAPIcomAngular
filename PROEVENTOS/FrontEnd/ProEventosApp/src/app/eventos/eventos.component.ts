import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.scss']
})
export class EventosComponent implements OnInit {

  public eventos: any = []; 
  widthImg: number = 60
  marginImg: number = 5

  constructor(private http:HttpClient) { }

  ngOnInit(): void {
    this.getEventos()
  }

  public getEventos(): void {

    this.http.get('https://localhost:7120/api/evento')
      .subscribe(
        response => this.eventos = response,
        error => console.log(error),
      );

  }
}
