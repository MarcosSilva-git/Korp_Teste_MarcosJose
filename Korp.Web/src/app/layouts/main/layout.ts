import { Component } from '@angular/core'
import { MatButtonModule } from '@angular/material/button'
import { MatToolbarModule } from '@angular/material/toolbar'
import { RouterModule } from '@angular/router'

@Component({
  selector: 'app-main-layout',
  imports: [RouterModule, MatToolbarModule, MatButtonModule],
  standalone: true,
  templateUrl: './layout.html',
  styleUrl: './layout.css'
})
export class MainLayout {

}