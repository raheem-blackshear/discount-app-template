import { Component, OnInit, ViewChild, Input } from '@angular/core';
import { Nationality } from '../../../models/nationality.model';
import { SetupService } from "../../../services/setup.service";
import { Utilities } from "../../../services/utilities";
import { AlertService, DialogType, MessageSeverity } from '../../../services/alert.service';
import { AppTranslationService } from "../../../services/app-translation.service";

@Component({
  selector: 'nationality-info',
    templateUrl: './nationality-info.component.html',
    styleUrls: ['./nationality-info.component.css']
})
export class NationalityInfoComponent implements OnInit {

    public changesSavedCallback: () => void;
    public changesFailedCallback: () => void;
    public changesCancelledCallback: () => void;


    private isEditMode = false;
    private isNewUser = false;
    private isSaving = false;
    private isEditingSelf = false;
    private showValidationErrors = false;
    private editingNationalityName: string;
    private uniqueId: string = Utilities.uniqueId();
    private nationality: Nationality = new Nationality();
    private nationalityEdit: Nationality = new Nationality();
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
            this.loadCurrentNationalityData();
        }
    }


    private loadCurrentNationalityData() {
        this.alertService.startLoadingMessage();
        this.setupService.getNationality().subscribe(nationality => this.onCurrentNationalityDataLoadSuccessful(nationality), error => this.onCurrentNationalityDataLoadFailed(error));
    }


    private onCurrentNationalityDataLoadSuccessful(nationality: Nationality) {
        this.alertService.stopLoadingMessage();
        this.nationality = nationality;
    }


    private onCurrentNationalityDataLoadFailed(error: any) {
        this.alertService.stopLoadingMessage();
        this.alertService.showStickyMessage("Load Error", `Unable to retrieve type consume data from the server.\r\nErrors: "${Utilities.getHttpResponseMessage(error)}"`,
            MessageSeverity.error, error);

        this.nationality = new Nationality();
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


    newNationality() {
        this.isGeneralEditor = true;
        this.isNewUser = true;

        this.editingNationalityName = null;
        this.nationality = this.nationalityEdit = new Nationality();
        this.edit();

        return this.nationalityEdit;
    }


    editNationality(nationality: Nationality) {
        if (nationality) {
            this.isGeneralEditor = true;
            this.isNewUser = false;

            this.editingNationalityName = nationality.nationalityName;
            this.nationality = new Nationality();
            this.nationalityEdit = new Nationality();
            Object.assign(this.nationality, nationality);
            Object.assign(this.nationalityEdit, nationality);
            this.edit();

            return this.nationalityEdit;
        }
        else {
            return this.newNationality();
        }
    }


    private edit() {
        if (!this.isGeneralEditor) {
            this.isEditingSelf = true;
            this.nationalityEdit = new Nationality();
            Object.assign(this.nationalityEdit, this.nationality);
        }
        else {
            if (!this.nationalityEdit)
                this.nationalityEdit = new Nationality();

            //  this.isEditingSelf = this.setupService.currentUser ? this.nationalityEdit.NationalityId == this.setupService.currentUser.id : false;
        }

        this.isEditMode = true;
        this.showValidationErrors = true;

    }


    displayNationality(nationality: Nationality) {

        this.nationality = new Nationality();
        Object.assign(this.nationality, nationality);

        this.isEditMode = false;
    }


    private showErrorAlert(caption: string, message: string) {
        this.alertService.showMessage(caption, message, MessageSeverity.error);
    }



    private save() {
        this.isSaving = true;
        this.alertService.startLoadingMessage("Saving changes...");

        if (this.isNewUser) {
            this.setupService.newNationality(this.nationalityEdit).subscribe(type => this.saveSuccessHelper(type), error => this.saveFailedHelper(error));
        }
        else {
          this.setupService.updateNationality(this.nationalityEdit).subscribe(response => this.saveSuccessHelper(), error => this.saveFailedHelper(error));
        }
    }

    private saveSuccessHelper(type?: Nationality) {

        if (type)
            Object.assign(this.nationalityEdit, type);

        this.isSaving = false;
        this.alertService.stopLoadingMessage();
        this.showValidationErrors = false;

        Object.assign(this.nationality, this.nationalityEdit);
        this.nationalityEdit = new Nationality();
        this.resetForm();


        if (this.isGeneralEditor) {
            if (this.isNewUser)
                this.alertService.showMessage("Success", `Type Consume \"${this.nationality.nationalityName}\" was created successfully`, MessageSeverity.success);
            else if (!this.isEditingSelf)
                this.alertService.showMessage("Success", `Changes to user \"${this.nationality.nationalityName}\" was saved successfully`, MessageSeverity.success);
        }

        if (this.isEditingSelf) {
            this.alertService.showMessage("Success", "Changes to your Type Consume was saved successfully", MessageSeverity.success);

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
            this.nationalityEdit = this.nationality = new Nationality();
        else
            this.nationalityEdit = new Nationality();

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
        this.nationalityEdit = this.nationality = new Nationality();
        this.showValidationErrors = false;
        this.resetForm();
        this.isEditMode = false;

        if (this.changesSavedCallback)
            this.changesSavedCallback();
    }





}
