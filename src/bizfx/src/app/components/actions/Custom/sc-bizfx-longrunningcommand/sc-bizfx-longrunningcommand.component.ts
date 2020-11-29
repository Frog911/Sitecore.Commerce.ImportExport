import { Component, OnInit, Input } from '@angular/core';
import { ScBizFxProperty, ScBizFxContextService, ScBizFxAuthService } from '@sitecore/bizfx';
import { FormGroup } from '@angular/forms';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'sc-bizfx-longrunningcommand',
  templateUrl: './sc-bizfx-longrunningcommand.component.html',
  styleUrls: ['./sc-bizfx-longrunningcommand.component.css']
})
export class ScBizfxLongrunningcommandComponent implements OnInit {

    /**
   * Defines the property to be render
   */
  @Input() property: ScBizFxProperty;
  /**
     * Defines the form group that maps to the action's view
     */
  @Input() actionForm: FormGroup;

  get params(): LongRunningCommandParams {
    return JSON.parse(this.property.Value);
  }

  status: LongRunningCommandStatus = LongRunningCommandStatus.Running;
  statusType = LongRunningCommandStatus;

  constructor(private bizFxContext: ScBizFxContextService, private authService: ScBizFxAuthService, private http: HttpClient) { }

  ngOnInit() {
    let requestSending = false;
    const update = setInterval(() => {
      if ( this.status !== LongRunningCommandStatus.Running) {
        clearInterval(update);
        return;
      }
      if (requestSending) {
        return;
      }
      requestSending = true;
      this.http.get(this.bizFxContext.config.EngineUri + `/commerceops/CheckCommandStatus(taskId=${this.params.LongRunningTaskId})`,
      {
        headers: this.authService.getHeadersWithAuth(),
        withCredentials: true
      }).subscribe((data: any) => {
        requestSending = false;
        if ( data.IsCompleted && !data.IsCanceled && data.ResponseCode !== 'Error') {
          this.status = LongRunningCommandStatus.Success;
        } else if (data.IsCompleted || data.IsCanceled || data.IsFaulted) {
          this.status = LongRunningCommandStatus.Error;
        }
      }, err => {
        console.error(err);
        this.status = LongRunningCommandStatus.Error;
      });
    }, this.params.UpdatePeriod);
  }

}

enum LongRunningCommandStatus {
  Running, Success, Error
}

class LongRunningCommandParams {
  LongRunningTaskId: number;
  UpdatePeriod: number;
  RunningMessage: string;
  SuccessMessage: string;
  ErrorMessage: string;
}
