import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '../../core/services/auth/auth.service';
import { MessageService } from '../../core/services/message/message.service';

@Component({
  selector: 'app-update-email',
  templateUrl: './update-email.component.html',
  styleUrls: ['./update-email.component.scss']
})
export class UpdateEmailComponent {
  private _id: string;
  private _code: string;

  constructor(private _route: ActivatedRoute, private _auth: AuthService, private _router: Router, private _message: MessageService) {
    const id = this._route.snapshot.paramMap.get('id');
    const code = this._route.snapshot.paramMap.get('code');
    if (id === null || Number.isNaN(Number(id)) || code === null)
      this._router.navigate(['/notfound']);
    this._id = id ?? '';
    this._code = code ?? '';
  }

  ngOnInit(): void {
    this._auth.updateEmail(this._id, this._code).subscribe({
      next: this.success.bind(this),
      error: this.error.bind(this)
    });
  }


  private success(): void {
    this._message.show('Email updated successfully');
    this._router.navigate(['/login']);
  }

  private error(): void {
    this._message.show('Email update failed');
    this._router.navigate(['/']);
  }
}
