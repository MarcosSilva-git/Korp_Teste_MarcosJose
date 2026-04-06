import { Component, signal } from '@angular/core';
import { RouterModule } from '@angular/router';

// @Component({
//   selector: 'app-root',
//   templateUrl: './app.html',
//   standalone: false,
//   styleUrl: './app.css'
// })
// export class App {
//   protected readonly title = signal('Korp.Web');
// }

@Component({
  selector: 'app-root',
  template: '<router-outlet></router-outlet>',
  imports: [RouterModule],
  standalone: true,
})
export class App {
   
}
