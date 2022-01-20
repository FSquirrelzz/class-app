/*import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot, UrlTree } from "@angular/router";
import { ToastrService } from "ngx-toastr";
import { Observable } from "rxjs";

@Injectable({
  providedIn:'root'
})
export class AuthGuard  implements CanActivate {
  constructor(private accountService:accountService,
    private toastr:ToastrService
    ) { }
  canActivate(): Observable<boolean>{
    return this.accountService.currentUser$.pipe(
      map(user=>{
        if (user) return true;
        this.toastr.error('You must be signed in to access this page.'));
        return false;
      })
    )
  }

}*/

// uncomment when im caught up to 19/01, missing too much to make this work
