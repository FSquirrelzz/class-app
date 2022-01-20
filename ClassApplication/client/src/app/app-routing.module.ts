import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { ListsComponent } from './lists/lists.component';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { MembersListComponent } from './members/members-list/members-list.component';
import { MessagesComponent } from './messages/messages.component';

const routes: Routes = [
  {
    path:'',
    component:HomeComponent,
    pathMatch:'full',
  },
  {
    path:'members',//localhost:4200/members
    component:MembersListComponent,
  },
  {
    path:'members/:id',//localhost:4200/members/3(id)
    component:MemberDetailComponent,
  },
  {
    path:'lists',//localhost:4200/lists
    component:ListsComponent
  },
  {
    path:'messages',//localhost:4200/messages
    component:MessagesComponent
  },
  {
    path:'**',//localhost:4200/asdasdskad (path that doesnt exist sends home)
    pathMatch:'full',
    component:HomeComponent//needs to send to 404 or something later instead to home
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
