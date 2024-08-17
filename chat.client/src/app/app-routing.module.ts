import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ChatComponent } from './chat/chat.component';
import { ChatListComponent } from './chat-list/chat-list.component';
import { RegisterComponent } from './register/register.component';
import { LoginComponent } from './login/login.component';
import { authGuard } from './auth.guard';
import { authLoginGuard } from './auth-login.guard';

const routes: Routes = [
  { path: "chat", component: ChatListComponent, canActivate: [authGuard] },
  { path: "auth/register", component: RegisterComponent,canActivate:[authLoginGuard] },
  { path: "auth/login", component: LoginComponent, canActivate: [authLoginGuard] },
  { path: "", redirectTo:"chat", pathMatch:"full" }

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
