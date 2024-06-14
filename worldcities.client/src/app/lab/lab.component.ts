import { Component, OnInit } from '@angular/core';
import { interval } from 'rxjs';

@Component({
    selector: 'app-lab',
    templateUrl: './lab.component.html',
    styleUrl: './lab.component.scss'
})
export class LabComponent implements OnInit {

    ngOnInit() {
        const sub = interval(1000).subscribe({
            next(num) {
                console.log(num);
            },
            complete() {
                console.log("complete");
            }
        });

        setTimeout(() => {
            sub.unsubscribe();
            console.log('unsubsribe!');
        }, 2500);
    }
}
