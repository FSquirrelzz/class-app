import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { ToastrModule } from 'ngx-toastr';
import {NgxGalleryModule} from '@kolkov/ngx-gallery';
import {NgxSpinnerModule} from 'ngx-spinner';

@NgModule({
  declarations: [],
  imports: [
    NgxGalleryModule,
    CommonModule,
    BsDropdownModule.forRoot(),
    ToastrModule.forRoot({
      positionClass:'toast-bottom-right'
    }),
    TabsModule.forRoot(),
    NgxSpinnerModule,
  ],
  exports:[
    NgxGalleryModule,
    TabsModule,
    BsDropdownModule,
    ToastrModule,
    NgxSpinnerModule
  ],
})
export class SharedModule { }
