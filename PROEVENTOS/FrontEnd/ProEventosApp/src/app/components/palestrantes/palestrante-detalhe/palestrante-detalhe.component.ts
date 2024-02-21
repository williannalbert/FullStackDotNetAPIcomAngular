import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Palestrante } from '@app/models/Palestrante';
import { PalestranteService } from '@app/services/palestrante.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { debounceTime, map, tap } from 'rxjs/operators';

@Component({
  selector: 'app-palestrante-detalhe',
  templateUrl: './palestrante-detalhe.component.html',
  styleUrls: ['./palestrante-detalhe.component.scss']
})
export class PalestranteDetalheComponent implements OnInit {

  public form!:FormGroup;
  public situacaoForm = '';
  public corDescricao = '';
  
  constructor(
    private fb: FormBuilder,
    public palestranService: PalestranteService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService
  ) { }

  ngOnInit() {
    this.validation();
    this.verificaForm();
    this.carregarPalesrante();
  }

  private validation(): void{
    this.form = this.fb.group({
      miniCurriculo: ['']
    });
  }

  public get f():any{
    return this.form.controls;
  }

  private verificaForm():void{
    this.form.valueChanges
      .pipe(
        map(() => {
          this.situacaoForm = 'Mini Currículo sendo atualizado';
          this.corDescricao = 'text-warning';
        }),
        debounceTime(1000),
        tap(() => this.spinner.show())
      ).subscribe(() => {
        this.palestranService.put({...this.form.value})
        .subscribe(
          () => {
            this.situacaoForm = 'Mini Curriculo atualizado';
            this.corDescricao = 'text-success';

            setTimeout(() => {
              this.situacaoForm = '';
              this.corDescricao = 'text-muted';
            }, 2000);
          },
          (error) => {
            this.toastr.error('Erro ao atualizar mini curriculo', 'Erro')
            console.error(error)
          }
        ).add(() => this.spinner.hide()) 
      })
    }

    private carregarPalesrante():void{
      this.spinner.show();
      this.palestranService
        .getPalestrante()
        .subscribe(
          (palestrante: Palestrante) => {
              this.form.patchValue(palestrante);
          },
          (error: any) => {
              this.toastr.error('Erro ao carregar palestrante', 'Erro')
          }
        )
    }
}
