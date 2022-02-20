import { Component, Input, OnInit } from '@angular/core';
import { Member } from 'src/app/models/member';
import { User } from 'src/app/models/user';
import { MembersService } from 'src/app/services/members.service';
import { FileUploader, FileUploaderOptions } from 'ng2-file-upload';
import { environment } from 'src/environments/environment';
import { AccountService } from 'src/app/services/account.service';
import { take } from 'rxjs';
import { Photo } from 'src/app/models/photo';
@Component({
  selector: 'app-photo-edit',
  templateUrl: './photo-edit.component.html',
  styleUrls: ['./photo-edit.component.css'],
})
export class PhotoEditComponent implements OnInit {
  @Input() member!: Member;

  uploader!: FileUploader;
  hasBaseDropZoneOver: boolean = false;
  baseUrl = environment.apiUrl;
  user!: User;

  constructor(private accountService: AccountService,private memberService: MembersService) {
    accountService.currentUser$
      .pipe(take(1))
      .subscribe((user) => (this.user = user as User));
  }

  ngOnInit(): void {
    this.initializeUploader();
  }

  initializeUploader() {
    const options: FileUploaderOptions ={
      url: `${this.baseUrl}users/add-photo`,
      authToken: `Bearer ${this.user.token}`,
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize:10 * 1024 * 1024,
    };
    this.uploader = new FileUploader(options);
    this.uploader.onAfterAddingFile = (file)=>{
      file.withCredentials=false;
    }
    this.uploader.onSuccessItem = (item , response,status,headers)=>
    {
      if(response)
      {
        const photo = JSON.parse(response);
        this.member.photos.push(photo);
      }
    }
  }
  public fileOverBase(e:any):void {
    this.hasBaseDropZoneOver = e;
  }

  public setMainPhoto(photo:Photo)
  {
    this.memberService.setMainPhoto(photo.id).subscribe(()=>{
      this.user.photoUrl=photo.url;
      this.accountService.setCurrentUser(this.user);
      this.member.photoUrl=photo.url;
      this.member.photos.forEach(p=>
        {
          p.isMain = photo.id==p.id;
        });
    })
  }
  deletePhoto(photoId:number){
    this.memberService.deletePhoto(photoId).subscribe(()=>{
      this.member.photos = this.member.photos.filter(p=>p.id!=photoId);
    });
  }
}
