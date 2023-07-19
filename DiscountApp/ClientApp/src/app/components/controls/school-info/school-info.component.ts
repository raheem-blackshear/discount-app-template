import { Component, OnInit, ViewChild, Input } from '@angular/core';
import { School } from '../../../models/school.model';
import { SchoolType } from '../../../models/schoolType.model';
import { SetupService } from "../../../services/setup.service";
import { Utilities } from "../../../services/utilities";
import { AlertService, DialogType, MessageSeverity } from '../../../services/alert.service';
import { AppTranslationService } from "../../../services/app-translation.service";
import { BootstrapSelectDirective } from "../../../directives/bootstrap-select.directive";

@Component({
  selector: 'school-info',
    templateUrl: './school-info.component.html',
    styleUrls: ['./school-info.component.css']
})
export class SchoolInfoComponent implements OnInit {

    @ViewChild("schoolTypeSelector")
    languageSelector: BootstrapSelectDirective;

    public changesSavedCallback: () => void;
    public changesFailedCallback: () => void;
    public changesCancelledCallback: () => void;

    private isEditMode = false;
    private isNewUser = false;
    private isSaving = false;
    private isEditingSelf = false;
    private showValidationErrors = false;
    private editingSchoolName: string;
    private uniqueId: string = Utilities.uniqueId();
    private school: School = new School();
    private schoolEdit: School = new School();
    public formResetToggle = true;
    //public allSchoolType: SchoolType = new SchoolType();
    public allSchoolType: any[] = [];

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
            this.loadCurrentSchoolData();
        }
    }


    private loadCurrentSchoolData() {
        this.alertService.startLoadingMessage();
        this.setupService.getSchool().subscribe(schools => this.onCurrentSchoolDataLoadSuccessful(schools), error => this.onCurrentSchoolDataLoadFailed(error));
        this.setupService.getSchoolType().subscribe(schoolTypes => this.onCurrentSchoolTypeDataLoadSuccessful(schoolTypes), error => this.onCurrentSchooTYpelDataLoadFailed(error));
    }


    private onCurrentSchoolTypeDataLoadSuccessful(schoolType: SchoolType) {
        this.alertService.stopLoadingMessage();

        var arr = Object.entries(schoolType).map(([type, value]) => ({ type, value }));

        this.allSchoolType = arr;
    }


    private onCurrentSchooTYpelDataLoadFailed(error: any) {
        this.alertService.stopLoadingMessage();
        this.alertService.showStickyMessage("Load Error", `Unable to retrieve school type data from the server.\r\nErrors: "${Utilities.getHttpResponseMessage(error)}"`,
            MessageSeverity.error, error);

        this.school = new School();
    }

    private onCurrentSchoolDataLoadSuccessful(school: School) {
        this.alertService.stopLoadingMessage();
        this.school = school;
    }


    private onCurrentSchoolDataLoadFailed(error: any) {
        this.alertService.stopLoadingMessage();
        this.alertService.showStickyMessage("Load Error", `Unable to retrieve school data from the server.\r\nErrors: "${Utilities.getHttpResponseMessage(error)}"`,
            MessageSeverity.error, error);

        this.school = new School();
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


    newSchool() {
        this.isGeneralEditor = true;
        this.isNewUser = true;

        this.editingSchoolName = null;
        this.school = this.schoolEdit = new School();
        this.edit();
        this.setupService.getSchoolType().subscribe(schoolTypes => this.onCurrentSchoolTypeDataLoadSuccessful(schoolTypes), error => this.onCurrentSchooTYpelDataLoadFailed(error));
        return this.schoolEdit;
    }


    editSchool(school: School) {
        if (school) {
            this.isGeneralEditor = true;
            this.isNewUser = false;

            this.editingSchoolName = school.schoolName;
            this.school = new School();
            this.schoolEdit = new School();
            Object.assign(this.school, school);
            Object.assign(this.schoolEdit, school);
            this.edit();
            this.setupService.getSchoolType().subscribe(schoolTypes => this.onCurrentSchoolTypeDataLoadSuccessful(schoolTypes), error => this.onCurrentSchooTYpelDataLoadFailed(error));
            return this.schoolEdit;
        }
        else {
            return this.newSchool();
        }
    }


    private edit() {
        if (!this.isGeneralEditor) {
            this.isEditingSelf = true;
            this.schoolEdit = new School();
            Object.assign(this.schoolEdit, this.school);
        }
        else {
            if (!this.schoolEdit)
                this.schoolEdit = new School();

            //  this.isEditingSelf = this.setupService.currentUser ? this.schoolEdit.SchoolId == this.setupService.currentUser.id : false;
        }

        this.isEditMode = true;
        this.showValidationErrors = true;

    }


    displaySchool(school: School) {

        this.school = new School();
        Object.assign(this.school, school);

        this.isEditMode = false;
    }


    private showErrorAlert(caption: string, message: string) {
        this.alertService.showMessage(caption, message, MessageSeverity.error);
    }



    private save() {
        this.isSaving = true;
        this.alertService.startLoadingMessage("Saving changes...");

        if (this.isNewUser) {
           this.setupService.newSchool(this.schoolEdit).subscribe(type => this.saveSuccessHelper(type), error => this.saveFailedHelper(error));
        }
        else {
            this.setupService.updateSchool(this.schoolEdit).subscribe(response => this.saveSuccessHelper(), error => this.saveFailedHelper(error));
        }
    }

    private saveSuccessHelper(type?: School) {

        if (type)
            Object.assign(this.schoolEdit, type);

        this.isSaving = false;
        this.alertService.stopLoadingMessage();
        this.showValidationErrors = false;

        Object.assign(this.school, this.schoolEdit);
        this.schoolEdit = new School();
        this.resetForm();


        if (this.isGeneralEditor) {
            if (this.isNewUser)
                this.alertService.showMessage("Success", `School \"${this.school.schoolName}\" was created successfully`, MessageSeverity.success);
            else if (!this.isEditingSelf)
                this.alertService.showMessage("Success", `Changes to user \"${this.school.schoolName}\" was saved successfully`, MessageSeverity.success);
        }

        if (this.isEditingSelf) {
            this.alertService.showMessage("Success", "Changes to your School was saved successfully", MessageSeverity.success);

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
            this.schoolEdit = this.school = new School();
        else
            this.schoolEdit = new School();

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
        this.schoolEdit = this.school = new School();
        this.showValidationErrors = false;
        this.resetForm();
        this.isEditMode = false;

        if (this.changesSavedCallback)
            this.changesSavedCallback();
    }





}
