import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '../../core/services/auth/auth.service';
import { MessageService } from '../../core/services/message/message.service';

@Component({
  selector: 'app-verify',
  templateUrl: './verify.component.html',
  styleUrls: ['./verify.component.scss']
})
export class VerifyComponent implements OnInit {
  private _id: string;
  private _code: string;

  constructor(private _route: ActivatedRoute, private _auth: AuthService, private _router: Router, private _message: MessageService) {
    const id = this._route.snapshot.paramMap.get('id');
    const code = this._route.snapshot.paramMap.get('code');
    if (id === null || Number.isNaN(Number(id)) || code === null)
      this._router.navigate(['/not-found']);
    this._id = id ?? '';
    this._code = code ?? '';
  }

  ngOnInit(): void {
    this._auth.verify(this._id, this._code).subscribe({
      next: this.success.bind(this),
      error: this.error.bind(this)
    });
  }

  private success(): void {
    this._message.show('Account verified successfully');
    this._router.navigate(['/login']);
  }

  private error(): void {
    this._message.show('Account verification failed');
    this._router.navigate(['/']);
  }
}
