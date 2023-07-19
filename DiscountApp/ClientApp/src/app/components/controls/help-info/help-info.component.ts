import { Component, OnInit, ViewChild, Input } from '@angular/core';
import { Help } from '../../../models/help.model';
import { SetupService } from "../../../services/setup.service";
import { Utilities } from "../../../services/utilities";
import { AlertService, DialogType, MessageSeverity } from '../../../services/alert.service';
import { AppTranslationService } from "../../../services/app-translation.service";

@Component({
  selector: 'help-info',
  templateUrl: './help-info.component.html',
  styleUrls: ['./help-info.component.css']
})
export class HelpInfoComponent implements OnInit {

    public changesSavedCallback: () => void;
    public changesFailedCallback: () => void;
    public changesCancelledCallback: () => void;


    private isEditMode = false;
    private isNewUser = false;
    private isSaving = false;
    private isEditingSelf = false;
    private showValidationErrors = false;
    private editingHelpName: string;
    private uniqueId: string = Utilities.uniqueId();
    private help: Help = new Help();
    private helpEdit: Help = new Help();
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
            this.loadCurrentHelpData();
        }
    }


    private loadCurrentHelpData() {
        this.alertService.startLoadingMessage();
        this.setupService.getHelp().subscribe(help => this.onCurrentHelpDataLoadSuccessful(help), error => this.onCurrentHelpDataLoadFailed(error));
    }


    private onCurrentHelpDataLoadSuccessful(help: Help) {
        this.alertService.stopLoadingMessage();
        this.help = help;
    }


    private onCurrentHelpDataLoadFailed(error: any) {
        this.alertService.stopLoadingMessage();
        this.alertService.showStickyMessage("Load Error", `Unable to retrieve help data from the server.\r\nErrors: "${Utilities.getHttpResponseMessage(error)}"`,
            MessageSeverity.error, error);

        this.help = new Help();
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


    newHelp() {
        this.isGeneralEditor = true;
        this.isNewUser = true;

        this.editingHelpName = null;
        this.help = this.helpEdit = new Help();
        this.edit();

        return this.helpEdit;
    }


    editHelp(help: Help) {
        if (help) {
            this.isGeneralEditor = true;
            this.isNewUser = false;

            this.editingHelpName = help.helpName;
            this.help = new Help();
            this.helpEdit = new Help();
            Object.assign(this.help, help);
            Object.assign(this.helpEdit, help);
            this.edit();

            return this.helpEdit;
        }
        else {
            return this.newHelp();
        }
    }


    private edit() {
        if (!this.isGeneralEditor) {
            this.isEditingSelf = true;
            this.helpEdit = new Help();
            Object.assign(this.helpEdit, this.help);
        }
        else {
            if (!this.helpEdit)
                this.helpEdit = new Help();

             //this.isEditingSelf = this.setupService.currentUser ? this.helpEdit.helpId == this.setupService.currentUser.id : false;
        }

        this.isEditMode = true;
        this.showValidationErrors = true;

    }


    displayHelp(help: Help) {

        this.help = new Help();
        Object.assign(this.help, help);

        this.isEditMode = false;
    }


    private showErrorAlert(caption: string, message: string) {
        this.alertService.showMessage(caption, message, MessageSeverity.error);
    }



    private save() {
        this.isSaving = true;
        this.alertService.startLoadingMessage("Saving changes...");

        if (this.isNewUser) {
            this.setupService.newHelp(this.helpEdit).subscribe(type => this.saveSuccessHelper(type), error => this.saveFailedHelper(error));
        }
        else {
            this.setupService.updateHelp(this.helpEdit).subscribe(response => this.saveSuccessHelper(), error => this.saveFailedHelper(error));
        }
    }

    private saveSuccessHelper(type?: Help) {

        if (type)
            Object.assign(this.helpEdit, type);

        this.isSaving = false;
        this.alertService.stopLoadingMessage();
        this.showValidationErrors = false;

        Object.assign(this.help, this.helpEdit);
        this.helpEdit = new Help();
        this.resetForm();


        if (this.isGeneralEditor) {
            if (this.isNewUser)
                this.alertService.showMessage("Success", `Help \"${this.help.helpName}\" was created successfully`, MessageSeverity.success);
            else if (!this.isEditingSelf)
                this.alertService.showMessage("Success", `Changes to user \"${this.help.helpName}\" was saved successfully`, MessageSeverity.success);
        }

        if (this.isEditingSelf) {
            this.alertService.showMessage("Success", "Changes to your Help was saved successfully", MessageSeverity.success);

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
            this.helpEdit = this.help = new Help();
        else
            this.helpEdit = new Help();

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
        this.helpEdit = this.help = new Help();
        this.showValidationErrors = false;
        this.resetForm();
        this.isEditMode = false;

        if (this.changesSavedCallback)
            this.changesSavedCallback();
    }





}
