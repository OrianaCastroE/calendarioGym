import { Component, Input } from '@angular/core';

export interface Stat {
  label: string;
  value: string;
  icon: string;
  description: string;
}

@Component({
  selector: 'app-stat-card',
  standalone: true,
  templateUrl: './stat-card.component.html',
  styleUrl: './stat-card.component.css'
})
export class StatCardComponent {
  @Input({ required: true }) stat!: Stat;
}
