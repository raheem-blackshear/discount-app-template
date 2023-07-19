// ====================================================
// More Templates: https://www.ebenmonney.com/templates
// Email: support@ebenmonney.com
// ====================================================

import { Component } from '@angular/core';
import { fadeInOut } from '../../services/animations';
import { ConfigurationService } from '../../services/configuration.service';


@Component({
    selector: 'home',
    templateUrl: './home.component.html',
    styleUrls: ['./home.component.css'],
    animations: [fadeInOut]
})
export class HomeComponent {
        appHeader = require("../../assets/images/header-image-irrigation.jpg");
        appMobile = require("../../assets/images/1.0.Splash.png");
        appGoogle = require("../../assets/images/Google Play Button.png");
        appStore = require("../../assets/images/App Store Button.png");
    constructor(public configurations: ConfigurationService) {
    }

   
}
