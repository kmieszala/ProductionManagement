import { Directive,  ElementRef, Renderer2, OnInit, Input } from '@angular/core';

@Directive({
	selector: '[userStatus]'
})
export class UserStatusDirective implements OnInit {


	constructor(private renderer: Renderer2, private el: ElementRef) {
	}

	ngOnInit(): void {
	}

	@Input() set userStatus(status: string) {
    switch(status) {
        case '1': // New
            this.generateBadge('Dodany', 'bg-primary');
            break;
        case '2': // Active
            this.generateBadge('Aktywny', 'bg-success');
            break;
        case '3': // TimeBlocked
            this.generateBadge('Zablokowany', 'bg-warning');
            break;
        case '4': // Deleted
            this.generateBadge('Usuniety', 'bg-danger');
            break;
    }
	}

	private generateBadge(text: string, badgeClass: string) {

		this.el.nativeElement.textContent = text;
		this.renderer.addClass(this.el.nativeElement, 'badge');
		this.renderer.addClass(this.el.nativeElement, 'badge-size');
		this.renderer.addClass(this.el.nativeElement, badgeClass);
	}
}
