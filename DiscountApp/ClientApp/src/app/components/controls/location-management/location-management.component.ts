import { Component, OnInit, AfterViewInit, TemplateRef, ViewChild, Input } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { Location } from '../../../models/location.model';
import { AlertService, DialogType, MessageSeverity } from '../../../services/alert.service';
import { AppTranslationService } from "../../../services/app-translation.service";
import { SetupService } from "../../../services/setup.service";
import { Utilities } from "../../../services/utilities";
import { LocationInfoComponent } from '../location-info/location-info.component';
@Component({
    selector: 'location-management',
    templateUrl: './location-management.component.html',
    styleUrls: ['./location-management.component.css']
})
export class LocationManagementComponent implements OnInit, AfterViewInit {

    loadingIndicator: boolean;
    columns: any[] = [];
    rows: Location[] = [];
    rowsCache: Location[] = [];
    editedLocation: Location;
    sourceLocation: Location;
    editingLocationName: { name: string };



    @ViewChild('indexTemplate')
    indexTemplate: TemplateRef<any>;

    @ViewChild('actionsTemplate')
    actionsTemplate: TemplateRef<any>;

    @ViewChild('locationEditor')
    locationEditor: LocationInfoComponent;

    @ViewChild('editorModal')
    editorModal: ModalDirective;



    constructor(private alertService: AlertService, private translationService: AppTranslationService, private setupService: SetupService) {
    }

    ngOnInit() {

        let gT = (key: string) => this.translationService.getTranslation(key);

      
        this.columns = [
            { prop: "index", name: '#', width: 50, cellTemplate: this.indexTemplate, canAutoResize: false },         
            { prop: 'locationName', name: gT('location.Name'), width: 200 },                  
            { name: '', width: 130, cellTemplate: this.actionsTemplate, resizeable: false, canAutoResize: false, sortable: false, draggable: false }
        ];
        
     

        this.loadData();
    }

    ngAfterViewInit() {

        this.locationEditor.changesSavedCallback = () => {
            this.addNewLocationToList();
            this.editorModal.hide();
        };

        this.locationEditor.changesCancelledCallback = () => {
            this.editedLocation = null;
            this.sourceLocation = null;
            this.editorModal.hide();
        };
    }

    addNewLocationToList() {
        if (this.sourceLocation) {
            Object.assign(this.sourceLocation, this.editedLocation);

            let sourceIndex = this.rowsCache.indexOf(this.sourceLocation, 0);
            if (sourceIndex > -1)
                Utilities.moveArrayItem(this.rowsCache, sourceIndex, 0);

            sourceIndex = this.rows.indexOf(this.sourceLocation, 0);
            if (sourceIndex > -1)
                Utilities.moveArrayItem(this.rows, sourceIndex, 0);

           this.editedLocation = null;
            this.sourceLocation = null;
        }
        else {
            let location = new Location();
            Object.assign(location, this.editedLocation);
            this.editedLocation = null;

            let maxIndex = 0;
            for (let u of this.rowsCache) {
                if ((<any>u).index > maxIndex)
                    maxIndex = (<any>u).index;
            }

            (<any>location).index = maxIndex + 1;

            this.rowsCache.splice(0, 0, location);
            this.rows.splice(0, 0, location);
            this.rows = [...this.rows];

        }

       this.loadData();
    }


    loadData() {
        this.alertService.startLoadingMessage();
        this.loadingIndicator = true;
        this.setupService.getLocations().subscribe(location => this.onDataLoadSuccessful(location), error => this.onDataLoadFailed(error));

    }


    onDataLoadFailed(error: any) {
        this.alertService.stopLoadingMessage();
        this.loadingIndicator = false;

        this.alertService.showStickyMessage("Load Error", `Unable to retrieve users from the server.\r\nErrors: "${Utilities.getHttpResponseMessage(error)}"`,
            MessageSeverity.error, error);
    }


    onDataLoadSuccessful(location: Location[]) {
        this.alertService.stopLoadingMessage();
        this.loadingIndicator = false;

        location.forEach((type, index, location) => {
            (<any>type).index = index + 1;
        });

        this.rowsCache = [...location];
        this.rows = location;

    }


    onSearchChanged(value: string) {
        this.rows = this.rowsCache.filter(r => Utilities.searchArray(value, false, r.locationName));
    }


    onEditorModalHidden() {
       this.editingLocationName = null;
        this.locationEditor.resetForm(true);
    }


    newLocation() {
       this.editingLocationName = null;
       this.sourceLocation = null;
        this.editedLocation = this.locationEditor.newLocation();
        this.editorModal.show();
    }


    editLocation(row: Location) {
        this.editingLocationName = { name: row.locationName };
        this.sourceLocation = row;
        this.editedLocation = this.locationEditor.editLocation(row);
        this.editorModal.show();
    }

    deleteLocation(row: Location) {

        this.alertService.showDialog('Are you sure you want to delete \"' + row.locationName + '\"?', DialogType.confirm, () => this.deleteLocationHelper(row));
    }


    deleteLocationHelper(row: Location) {

        this.alertService.startLoadingMessage("Deleting...");
        this.loadingIndicator = true;

        this.setupService.deleteLocation(row)
            .subscribe(results => {
                this.alertService.stopLoadingMessage();
                this.loadingIndicator = false;

                this.rowsCache = this.rowsCache.filter(item => item !== row)
                this.rows = this.rows.filter(item => item !== row)
            },
            error => {
                this.alertService.stopLoadingMessage();
                this.loadingIndicator = false;

                this.alertService.showStickyMessage("Delete Error", `An error occured whilst deleting the user.\r\nError: "${Utilities.getHttpResponseMessage(error)}"`,
                    MessageSeverity.error, error);
         });
    }

}
