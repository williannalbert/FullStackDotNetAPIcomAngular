import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-palestrantes',
  templateUrl: './palestrantes.component.html',
  styleUrls: ['./palestrantes.component.css']
})
export class PalestrantesComponent implements OnInit {
  public palestrantes: any;
  constructor() { }

  ngOnInit() {
    this.getPalestrantes()
  }

  public getPalestrantes(){
    this.palestrantes = [
      {
        nome: 'João Victor',
        especializacao: 'Programador mobile'
      },
      {
        nome: 'Felipe Santos',
        especializacao: 'Administrador de banco de dados'
      }
    ]
  }  
}
