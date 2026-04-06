import { Component } from '@angular/core'
// import { MatButtonModule } from '@angular/material/button'
// import { MatToolbarModule } from '@angular/material/toolbar'
import { RouterModule } from '@angular/router'

@Component({
  selector: 'app-main-layout',
//   imports: [MatToolbarModule, RouterModule, MatButtonModule],
  imports: [RouterModule],
  standalone: true,
  templateUrl: './layout.html'
})
export class MainLayout {

}