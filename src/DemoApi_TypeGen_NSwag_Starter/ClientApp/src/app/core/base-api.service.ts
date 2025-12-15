//----------------------
// Base API Service for common HTTP operations
//----------------------

import { HttpClient, HttpHeaders, HttpResponse, HttpResponseBase } from '@angular/common/http';
import { Observable, throwError as _observableThrow, of as _observableOf } from 'rxjs';
import { mergeMap as _observableMergeMap } from 'rxjs/operators';
import { SwaggerException } from '../core';

export abstract class BaseApiService {
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

    constructor(protected http: HttpClient, protected baseUrl: string) {}

    protected get<T>(url: string): Observable<T> {
        const fullUrl = this.baseUrl + url;
        
        const options = {
            observe: "response" as const,
            responseType: "blob" as const,
            headers: new HttpHeaders({
                "Accept": "application/json"
            })
        };

        return this.http.request("get", fullUrl, options).pipe(
            _observableMergeMap((response: any) => this.processResponse<T>(response))
        );
    }

    protected post<T>(url: string, body: any): Observable<T> {
        const fullUrl = this.baseUrl + url;
        
        const options = {
            observe: "response" as const,
            responseType: "blob" as const,
            headers: new HttpHeaders({
                "Accept": "application/json",
                "Content-Type": "application/json"
            }),
            body: JSON.stringify(body)
        };

        return this.http.request("post", fullUrl, options).pipe(
            _observableMergeMap((response: any) => this.processResponse<T>(response))
        );
    }

    protected put<T>(url: string, body: any): Observable<T> {
        const fullUrl = this.baseUrl + url;
        
        const options = {
            observe: "response" as const,
            responseType: "blob" as const,
            headers: new HttpHeaders({
                "Accept": "application/json",
                "Content-Type": "application/json"
            }),
            body: JSON.stringify(body)
        };

        return this.http.request("put", fullUrl, options).pipe(
            _observableMergeMap((response: any) => this.processResponse<T>(response))
        );
    }

    protected delete<T>(url: string): Observable<T> {
        const fullUrl = this.baseUrl + url;
        
        const options = {
            observe: "response" as const,
            responseType: "blob" as const,
            headers: new HttpHeaders({
                "Accept": "application/json"
            })
        };

        return this.http.request("delete", fullUrl, options).pipe(
            _observableMergeMap((response: any) => this.processResponse<T>(response))
        );
    }

    protected processResponse<T>(response: HttpResponseBase): Observable<T> {
        const status = response.status;
        const responseBlob = response instanceof HttpResponse ? response.body :
            (response as any).error instanceof Blob ? (response as any).error : undefined;

        const headers: any = {};
        if (response.headers) {
            for (const key of response.headers.keys()) {
                headers[key] = response.headers.get(key);
            }
        }

        if (status === 200) {
            return this.blobToText(responseBlob).pipe(_observableMergeMap(responseText => {
                const result = responseText === "" ? null : JSON.parse(responseText, this.jsonParseReviver) as T;
                return _observableOf(result as T);
            }));
        } else if (status === 204) {
            return _observableOf(null as T);
        } else {
            return this.blobToText(responseBlob).pipe(_observableMergeMap(responseText => {
                return this.throwException("An unexpected server error occurred.", status, responseText, headers);
            }));
        }
    }

    protected blobToText(blob: any): Observable<string> {
        return new Observable<string>((observer: any) => {
            if (!blob) {
                observer.next("");
                observer.complete();
            } else {
                const reader = new FileReader();
                reader.onload = event => {
                    observer.next((event.target as any).result);
                    observer.complete();
                };
                reader.readAsText(blob);
            }
        });
    }

    protected throwException(message: string, status: number, response: string, headers: { [key: string]: any; }, result?: any): Observable<any> {
        return _observableThrow(new SwaggerException(message, status, response, headers, result));
    }
}
