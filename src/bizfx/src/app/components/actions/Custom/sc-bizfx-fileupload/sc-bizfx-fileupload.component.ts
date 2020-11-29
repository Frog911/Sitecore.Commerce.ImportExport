import { Component, OnInit, Input } from '@angular/core';
import { ScBizFxProperty } from '@sitecore/bizfx';
import { FormGroup } from '@angular/forms';

@Component({
  selector: 'sc-bizfx-fileupload',
  templateUrl: './sc-bizfx-fileupload.component.html',
  styleUrls: ['./sc-bizfx-fileupload.component.css']
})
export class ScBizfxFileuploadComponent implements OnInit {

  /**
   * Defines the property to be render
   */
  @Input() property: ScBizFxProperty;
  /**
     * Defines the form group that maps to the action's view
     */
  @Input() actionForm: FormGroup;

  constructor() { }

  ngOnInit() {
  }

  onFileChange(event) {
    const file = event.target.files[0];
    const reader = new FileReader();
    reader.onload = ((theFile) => {
      return (e) => {
        let result = e.target.result;
        let encoded = reader.result.toString().replace(/^data:(.*,)?/, '');
        if ((encoded.length % 4) > 0) {
          encoded += '='.repeat(4 - (encoded.length % 4));
        }
        result = encoded;
        console.log('file content', result);
        this.actionForm.controls[this.property.Name].setValue(result);
      };
    })(file);
    reader.readAsDataURL(file);
  }
}
