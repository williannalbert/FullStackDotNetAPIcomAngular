import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Evento } from '../models/Evento';
import { take } from 'rxjs/operators'
import { environment } from '@environment/environment';

@Injectable(
  //{providedIn: 'root'}
  )
export class EventoService {

  baseURL = environment.apiURL+'api/evento';
  tokenHeader = new HttpHeaders({
    'Authorization' : 'Bearer eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiIxIiwidW5pcXVlX25hbWUiOiJ3aWxsaWFuIiwibmJmIjoxNzAyMzc0NjEzLCJleHAiOjE3MDI0NjEwMTMsImlhdCI6MTcwMjM3NDYxM30.xeFOX6_cT7G0_VIL07ltaMvo8Fp57lgB2Q378WlPgWHmjsZFRWHOCeVAlLW8MFD9Cz8eBdZHRtkqS9oltjd3mQ'
  })

constructor(private http:HttpClient) { }
  public getEventos():Observable<Evento[]>{
    return this.http
    .get<Evento[]>(this.baseURL, {headers: this.tokenHeader}).
    pipe(take(1));
  }

  public getEventosByTema(tema: string):Observable<Evento[]>{
    return this.http
    .get<Evento[]>(`${this.baseURL}/tema/${tema}`)
    .pipe(take(1));
  }

  public getEventoById(id: number):Observable<Evento>{
    return this.http
    .get<Evento>(`${this.baseURL}/${id}`)
    .pipe(take(1));
  }

  public post(evento: Evento):Observable<Evento>{
    return this.http
    .post<Evento>(this.baseURL, evento)
    .pipe(take(1));
  }
  public put(evento: Evento):Observable<Evento>{
    return this.http
    .put<Evento>(`${this.baseURL}/${evento.id}`, evento)
    .pipe(take(1));
  }
  public deleteEvento(id: number):Observable<any>{
    return this.http
    .delete(`${this.baseURL}/${id}`)
    .pipe(take(1));
  }
  postUpload(eventoId: number, file: File):Observable<Evento>{
    const fileUpload = file[0] as File;
    const formData = new FormData();
    formData.append('file', fileUpload)

    return this.http
    .post<Evento>(`${this.baseURL}/upload-imagem/${eventoId}`, formData)
    .pipe(take(1));
  }
}
