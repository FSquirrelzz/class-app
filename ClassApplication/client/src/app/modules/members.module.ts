import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MembersListComponent } from '../members/members-list/members-list.component';
import { MemberDetailComponent } from '../members/member-detail/member-detail.component';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes= [
  {path:'',component:MembersListComponent,pathMatch:'full'},
  {path:':id',component:MemberDetailComponent},
];
@NgModule({
  declarations: [MembersListComponent, MemberDetailComponent],
  imports: [CommonModule,RouterModule.forChild(routes)],
  exports:[MembersListComponent,MemberDetailComponent,RouterModule]
})
export class MembersModule {}
