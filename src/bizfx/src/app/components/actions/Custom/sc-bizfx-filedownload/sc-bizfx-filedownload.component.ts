import { Component, OnInit, AfterViewInit, Input, ElementRef, ChangeDetectorRef } from '@angular/core';
import { ScBizFxProperty, ScBizFxContextService, ScBizFxAuthService } from '@sitecore/bizfx';
import { FormGroup } from '@angular/forms';
import { saveAs } from 'file-saver';
import { Observable } from 'rxjs/Observable';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Component({
  selector: 'sc-bizfx-filedownload',
  templateUrl: './sc-bizfx-filedownload.component.html',
  styleUrls: ['./sc-bizfx-filedownload.component.css']
})
export class ScBizfxFiledownloadComponent implements AfterViewInit {
  /**
     * Defines the property to be render
     */
  @Input() property: ScBizFxProperty;
  /**
     * Defines the form group that maps to the action's view
     */
  @Input() actionForm: FormGroup;

  @Input() showRibbon: boolean;

  downloading = false;

  get downloadFileProps(): DownloadFileProps {
    return JSON.parse(this.property.Value);
  }

  constructor(private bizFxContext: ScBizFxContextService, private authService: ScBizFxAuthService, private http: HttpClient) {
  }

  ngAfterViewInit(): void {

  }

  download(e) {
    e.preventDefault();
    this.downloading = true;
    this.downloadReport().subscribe(
      data => {
        this.downloading = false;
        saveAs(new Blob([data]), this.downloadFileProps.FileName || 'result.txt');
      },
      err => {
        console.error(err);
      }
    );
  }

  downloadReport(): Observable<any> {
    const url = this.downloadFileProps.Action;
    return this.http.request(this.downloadFileProps.Method, this.bizFxContext.config.EngineUri + url,
      {
        body: this.downloadFileProps.Body,
        headers: this.authService.getHeadersWithAuth(),
        withCredentials: true,
        responseType: 'blob',
      });
  }
}


class DownloadFileProps {
  Method: string;
  Action: string;
  FileName: string;
  Body: any;
}
