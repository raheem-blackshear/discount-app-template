import { Component, OnInit, ViewChild, Input } from '@angular/core';
import { Location } from '../../../models/location.model';
import { SetupService } from "../../../services/setup.service";
import { Utilities } from "../../../services/utilities";
import { AlertService, DialogType, MessageSeverity } from '../../../services/alert.service';
import { AppTranslationService } from "../../../services/app-translation.service";

@Component({
  selector: 'location-info',
  templateUrl: './location-info.component.html',
    styleUrls: ['./location-info.component.css']
})
export class LocationInfoComponent implements OnInit {

    public changesSavedCallback: () => void;
    public changesFailedCallback: () => void;
    public changesCancelledCallback: () => void;


    private isEditMode = false;
    private isNewUser = false;
    private isSaving = false;
    private isEditingSelf = false;
    private showValidationErrors = false;
    private editingLocationName: string;
    private uniqueId: string = Utilities.uniqueId();
    private location: Location = new Location();
    private locationEdit: Location = new Location();
    public formResetToggle = true;

    @Input()
    isViewOnly: boolean;

    @Input()
    isGeneralEditor = false;

    @ViewChild('f')
    private form;

    constructor(private alertService: AlertService, private translationService: AppTranslationService, private setupService: SetupService) {

    }

    ngOnInit() {
        if (!this.isGeneralEditor) {
            this.loadCurrentLocationData();
        }
    }


    private loadCurrentLocationData() {
        this.alertService.startLoadingMessage();
        this.setupService.getLocation().subscribe(location => this.onCurrentLocationDataLoadSuccessful(location), error => this.onCurrentLocationDataLoadFailed(error));
    }


    private onCurrentLocationDataLoadSuccessful(location: Location) {
        this.alertService.stopLoadingMessage();
        this.location = location;
    }


    private onCurrentLocationDataLoadFailed(error: any) {
        this.alertService.stopLoadingMessage();
        this.alertService.showStickyMessage("Load Error", `Unable to retrieve location data from the server.\r\nErrors: "${Utilities.getHttpResponseMessage(error)}"`,
            MessageSeverity.error, error);

        this.location = new Location();
    }


    resetForm(replace = false) {

        if (!replace) {
            this.form.reset();
        }
        else {
            this.formResetToggle = false;

            setTimeout(() => {
                this.formResetToggle = true;
            });
        }
    }


    newLocation() {
        this.isGeneralEditor = true;
        this.isNewUser = true;

        this.editingLocationName = null;
        this.location = this.locationEdit = new Location();
        this.edit();

        return this.locationEdit;
    }


    editLocation(location: Location) {
        if (location) {
            this.isGeneralEditor = true;
            this.isNewUser = false;

            this.editingLocationName = location.locationName;
            this.location = new Location();
            this.locationEdit = new Location();
            Object.assign(this.location, location);
            Object.assign(this.locationEdit, location);
            this.edit();

            return this.locationEdit;
        }
        else {
            return this.newLocation();
        }
    }


    private edit() {
        if (!this.isGeneralEditor) {
            this.isEditingSelf = true;
            this.locationEdit = new Location();
            Object.assign(this.locationEdit, this.location);
        }
        else {
            if (!this.locationEdit)
                this.locationEdit = new Location();

             //this.isEditingSelf = this.setupService.currentUser ? this.locationEdit.locationId == this.setupService.currentUser.id : false;
        }

        this.isEditMode = true;
        this.showValidationErrors = true;

    }


    displayLocation(location: Location) {

        this.location = new Location();
        Object.assign(this.location, location);

        this.isEditMode = false;
    }


    private showErrorAlert(caption: string, message: string) {
        this.alertService.showMessage(caption, message, MessageSeverity.error);
    }



    private save() {
        this.isSaving = true;
        this.alertService.startLoadingMessage("Saving changes...");

        if (this.isNewUser) {
            this.setupService.newLocation(this.locationEdit).subscribe(type => this.saveSuccessHelper(type), error => this.saveFailedHelper(error));
        }
        else {
            this.setupService.updateLocation(this.locationEdit).subscribe(response => this.saveSuccessHelper(), error => this.saveFailedHelper(error));
        }
    }

    private saveSuccessHelper(type?: Location) {

        if (type)
            Object.assign(this.locationEdit, type);

        this.isSaving = false;
        this.alertService.stopLoadingMessage();
        this.showValidationErrors = false;

        Object.assign(this.location, this.locationEdit);
        this.locationEdit = new Location();
        this.resetForm();


        if (this.isGeneralEditor) {
            if (this.isNewUser)
                this.alertService.showMessage("Success", `Location \"${this.location.locationName}\" was created successfully`, MessageSeverity.success);
            else if (!this.isEditingSelf)
                this.alertService.showMessage("Success", `Changes to user \"${this.location.locationName}\" was saved successfully`, MessageSeverity.success);
        }

        if (this.isEditingSelf) {
            this.alertService.showMessage("Success", "Changes to your Location was saved successfully", MessageSeverity.success);

        }

        this.isEditMode = false;


        if (this.changesSavedCallback)
            this.changesSavedCallback();
    }


    private saveFailedHelper(error: any) {
        this.isSaving = false;
        this.alertService.stopLoadingMessage();
        this.alertService.showStickyMessage("Save Error", "The below errors occured whilst saving your changes:", MessageSeverity.error, error);
        this.alertService.showStickyMessage(error, null, MessageSeverity.error);

        if (this.changesFailedCallback)
            this.changesFailedCallback();
    }



    private cancel() {
        if (this.isGeneralEditor)
            this.locationEdit = this.location = new Location();
        else
            this.locationEdit = new Location();

        this.showValidationErrors = false;
        this.resetForm();

        this.alertService.showMessage("Cancelled", "Operation cancelled by user", MessageSeverity.default);
        this.alertService.resetStickyMessage();

        if (!this.isGeneralEditor)
            this.isEditMode = false;

        if (this.changesCancelledCallback)
            this.changesCancelledCallback();
    }


    private close() {
        this.locationEdit = this.location = new Location();
        this.showValidationErrors = false;
        this.resetForm();
        this.isEditMode = false;

        if (this.changesSavedCallback)
            this.changesSavedCallback();
    }





}
