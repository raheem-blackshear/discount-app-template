import { Component, OnInit, ViewChild, Input } from '@angular/core';
import { TypeConsume } from '../../../models/typeConsume.model';
import { SetupService } from "../../../services/setup.service";
import { Utilities } from "../../../services/utilities";
import { AlertService, DialogType, MessageSeverity } from '../../../services/alert.service';
import { AppTranslationService } from "../../../services/app-translation.service";

@Component({
  selector: 'type-consumes-info',
  templateUrl: './type-consumes-info.component.html',
  styleUrls: ['./type-consumes-info.component.css']
})
export class TypeConsumesInfoComponent implements OnInit {

    public changesSavedCallback: () => void;
    public changesFailedCallback: () => void;
    public changesCancelledCallback: () => void;


    private isEditMode = false;
    private isNewUser = false;
    private isSaving = false;
    private isEditingSelf = false;
    private showValidationErrors = false;
    private editingTypeConsumeName: string;
    private uniqueId: string = Utilities.uniqueId();
    private typeConsume: TypeConsume = new TypeConsume();
    private typeConsumeEdit: TypeConsume = new TypeConsume();
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
            this.loadCurrentTypeConsumeData();
        }
    }


    private loadCurrentTypeConsumeData() {
        this.alertService.startLoadingMessage();
        this.setupService.getTypeConsume().subscribe(typeConsumes => this.onCurrentTypeConsumeDataLoadSuccessful(typeConsumes), error => this.onCurrentTypeConsumeDataLoadFailed(error));
    }


    private onCurrentTypeConsumeDataLoadSuccessful(typeConsume: TypeConsume) {
        this.alertService.stopLoadingMessage();
        this.typeConsume = typeConsume;
    }


    private onCurrentTypeConsumeDataLoadFailed(error: any) {
        this.alertService.stopLoadingMessage();
        this.alertService.showStickyMessage("Load Error", `Unable to retrieve type consume data from the server.\r\nErrors: "${Utilities.getHttpResponseMessage(error)}"`,
            MessageSeverity.error, error);

        this.typeConsume = new TypeConsume();
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


    newTypeConsume() {
        this.isGeneralEditor = true;
        this.isNewUser = true;

        this.editingTypeConsumeName = null;
        this.typeConsume = this.typeConsumeEdit = new TypeConsume();
        this.edit();

        return this.typeConsumeEdit;
    }


    editTypeConsume(typeConsume: TypeConsume) {
        if (typeConsume) {
            this.isGeneralEditor = true;
            this.isNewUser = false;

            this.editingTypeConsumeName = typeConsume.typeConsumeName;
            this.typeConsume = new TypeConsume();
            this.typeConsumeEdit = new TypeConsume();
            Object.assign(this.typeConsume, typeConsume);
            Object.assign(this.typeConsumeEdit, typeConsume);
            this.edit();

            return this.typeConsumeEdit;
        }
        else {
            return this.newTypeConsume();
        }
    }


    private edit() {
        if (!this.isGeneralEditor) {
            this.isEditingSelf = true;
            this.typeConsumeEdit = new TypeConsume();
            Object.assign(this.typeConsumeEdit, this.typeConsume);
        }
        else {
            if (!this.typeConsumeEdit)
                this.typeConsumeEdit = new TypeConsume();

            //  this.isEditingSelf = this.setupService.currentUser ? this.typeConsumeEdit.TypeConsumeId == this.setupService.currentUser.id : false;
        }

        this.isEditMode = true;
        this.showValidationErrors = true;

    }


    displayTypeConsume(typeConsume: TypeConsume) {

        this.typeConsume = new TypeConsume();
        Object.assign(this.typeConsume, typeConsume);

        this.isEditMode = false;
    }


    private showErrorAlert(caption: string, message: string) {
        this.alertService.showMessage(caption, message, MessageSeverity.error);
    }



    private save() {
        this.isSaving = true;
        this.alertService.startLoadingMessage("Saving changes...");

        if (this.isNewUser) {
            this.setupService.newTypeConsume(this.typeConsumeEdit).subscribe(type => this.saveSuccessHelper(type), error => this.saveFailedHelper(error));
        }
        else {
            this.setupService.updateTypeConsume(this.typeConsumeEdit).subscribe(response => this.saveSuccessHelper(), error => this.saveFailedHelper(error));
        }
    }

    private saveSuccessHelper(type?: TypeConsume) {

        if (type)
            Object.assign(this.typeConsumeEdit, type);

        this.isSaving = false;
        this.alertService.stopLoadingMessage();
        this.showValidationErrors = false;

        Object.assign(this.typeConsume, this.typeConsumeEdit);
        this.typeConsumeEdit = new TypeConsume();
        this.resetForm();


        if (this.isGeneralEditor) {
            if (this.isNewUser)
                this.alertService.showMessage("Success", `Type Consume \"${this.typeConsume.typeConsumeName}\" was created successfully`, MessageSeverity.success);
            else if (!this.isEditingSelf)
                this.alertService.showMessage("Success", `Changes to user \"${this.typeConsume.typeConsumeName}\" was saved successfully`, MessageSeverity.success);
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
            this.typeConsumeEdit = this.typeConsume = new TypeConsume();
        else
            this.typeConsumeEdit = new TypeConsume();

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
        this.typeConsumeEdit = this.typeConsume = new TypeConsume();
        this.showValidationErrors = false;
        this.resetForm();
        this.isEditMode = false;

        if (this.changesSavedCallback)
            this.changesSavedCallback();
    }





}
