import { Directive,  ElementRef, Renderer2, OnInit, Input } from '@angular/core';

@Directive({
	selector: '[statusActive]'
})
export class StatusDirective implements OnInit {


	constructor(private renderer: Renderer2, private el: ElementRef) {
	}

	ngOnInit(): void {
	}

	@Input() set statusActive(status: string) {
    var boolValue = JSON.parse(status);
		if ( boolValue) {
			this.generateBadge('Aktywny', 'bg-success');
		}
		else {
			this.generateBadge('Nieaktywny', 'bg-danger');
		}
	}

	private generateBadge(text: string, badgeClass: string) {

		this.el.nativeElement.textContent = text;
		this.renderer.addClass(this.el.nativeElement, 'badge');
		this.renderer.addClass(this.el.nativeElement, 'badge-size');
		this.renderer.addClass(this.el.nativeElement, badgeClass);
	}
}
