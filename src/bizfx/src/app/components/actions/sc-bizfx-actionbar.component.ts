import { Component, Input, Output, EventEmitter, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { Location } from '@angular/common';

import { ScDialogService } from '@speak/ng-bcl/dialog';

import { ScBizFxAction, ScBizFxView } from '@sitecore/bizfx';
import { ScBizFxActionComponent } from './sc-bizfx-action.component';
import { ScBizFxViewsService } from '@sitecore/bizfx';

/**
 * BizFx View Action Bar `Component`.
 *
 * Renders an action bar for a view.
 */
@Component({
  selector: 'sc-bizfx-actionbar',
  templateUrl: './sc-bizfx-actionbar.component.html'
})

/**
 * BizFx View Action Bar `Component`.
 */
export class ScBizFxActionBarComponent {
  /**
     * Defines the view to be render
     */
  @Input() view: ScBizFxView;

  /**
    * @ignore
    */
  constructor(
    private location: Location,
    private router: Router,
    public dialogService: ScDialogService,
    private viewsService: ScBizFxViewsService) {
  }

  /**
   * Handles the action click
   */
  doAction(action: ScBizFxAction) {
    if (action.UiHint === 'RelatedList') {
      this.selectAction(action);
    } else if (action.IsMultiStep && action.FirstStepAction.UiHint === 'InlineAction') {
      const view = this.createViewForAction(action);
      this.viewsService.doAction(view).then(actionResult => {
        if (actionResult.ResponseCode === 'Ok') {
          action.IsMultiStep = false;
          action.FirstStepAction = null;
          this.openActionDialog(action);
        }
      });
    } else {
      this.openActionDialog(action);
    }
  }

  /**
   * Helper method
   *
   * Opens a dialog
   */
  protected openActionDialog(action: ScBizFxAction) {
    const view = this.createViewForAction(action);
    this.dialogService.open(ScBizFxActionComponent);
    const dialog: ScBizFxActionComponent = this.dialogService.contentComponentRef.instance;
    dialog.data = view;
    dialog.submitted
      .subscribe(() => {
        this.viewsService.announceAction(action);
      });
  }

  /**
  * Helper method
  *
  * Navigates to a view
  */
  protected selectAction(action: ScBizFxAction): void {
    if (action.UiHint === 'RelatedList') {
      this.router.navigate(['/entityView', action.EntityView]);
    } else {
      if (action.IsMultiStep) {
        this.router.navigate([
          '/action',
          action.FirstStepAction.EntityView,
          this.view.EntityId,
          action.FirstStepAction.Name,
          this.view.ItemId ? this.view.ItemId : '']);
      } else {
        this.router.navigate([
          '/action',
          action.EntityView,
          this.view.EntityId,
          action.Name,
          this.view.ItemId ? this.view.ItemId : '']);
      }
    }
  }

  /**
   * Creates a view for specific action
   *
   * @returns a view with defined properties for specific action.
   */
  createViewForAction(action: ScBizFxAction): ScBizFxView {
    const view = new ScBizFxView(
      this.view.EntityId !== null && this.view.EntityId !== undefined && this.view.EntityId !== 'undefined' ? this.view.EntityId : '',
      this.view.ItemId !== null && this.view.ItemId !== undefined && this.view.ItemId !== 'undefined' ? this.view.ItemId : '');

    if (action.IsMultiStep) {
      view.Name = action.FirstStepAction.EntityView;
      view.Action = action.FirstStepAction.Name;
    } else {
      view.Name = action.EntityView;
      view.Action = action.Name;
    }

    view.Actions = [];
    view.Actions.push(action);
    view.EntityVersion = this.view.EntityVersion;

    return view;
  }

  // ADDING THIS METHOD TO BE ABLE TO RID OFF FAVORITES
  /**
   * Helper method
   *
   * @returns a Computed tooltip which is tooltip property or defaults to text if no tooltip is defined.
   */
  tooltipAttribute(action: ScBizFxAction): string {
    return action.Description || action.DisplayName;
  }

  // ADDING THIS METHOD TO BE ABLE TO RID OFF FAVORITES
  /**
   * Helper method that contains logic for when to auto-promote actions.
   *
   * Should auto-promote all actions if there is 3 or less actions.
   * @returns Returns true when there is maximum 3 actions in total.
   */
  shouldAutoPromoteActions(): boolean {
    return this.view && this.view.Actions.length < 4;
  }
}
