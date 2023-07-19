import { Component, OnInit, AfterViewInit, TemplateRef, ViewChild, Input } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { School } from '../../../models/school.model';
import { AlertService, DialogType, MessageSeverity } from '../../../services/alert.service';
import { AppTranslationService } from "../../../services/app-translation.service";
import { SetupService } from "../../../services/setup.service";
import { Utilities } from "../../../services/utilities";
import { SchoolInfoComponent } from '../school-info/school-info.component';
@Component({
    selector: 'school-management',
    templateUrl: './school-management.component.html',
    styleUrls: ['./school-management.component.css']
})
export class SchoolManagementComponent implements OnInit, AfterViewInit {

    loadingIndicator: boolean;
    columns: any[] = [];
    rows: School[] = [];
    rowsCache: School[] = [];
    editedSchool: School;
    sourceSchool: School;
    editingSchoolName: { name: string };



    @ViewChild('indexTemplate')
    indexTemplate: TemplateRef<any>;

    @ViewChild('actionsTemplate')
    actionsTemplate: TemplateRef<any>;

    @ViewChild('schoolEditor')
    schoolEditor: SchoolInfoComponent;

    @ViewChild('editorModal')
    editorModal: ModalDirective;



    constructor(private alertService: AlertService, private translationService: AppTranslationService, private setupService: SetupService) {
    }

    ngOnInit() {

        let gT = (key: string) => this.translationService.getTranslation(key);

      
        this.columns = [
            { prop: "index", name: '#', width: 50, cellTemplate: this.indexTemplate, canAutoResize: false },         
            { prop: 'schoolName', name: gT('school.Name'), width: 200 },
            { prop: 'schoolValue', name: gT('school.Value'), width: 350 },          
            { name: '', width: 130, cellTemplate: this.actionsTemplate, resizeable: false, canAutoResize: false, sortable: false, draggable: false }
        ];
        
     

        this.loadData();
    }

    ngAfterViewInit() {

        this.schoolEditor.changesSavedCallback = () => {
            this.addNewSchoolToList();
            this.editorModal.hide();
        };

        this.schoolEditor.changesCancelledCallback = () => {
            this.editedSchool = null;
            this.sourceSchool = null;
            this.editorModal.hide();
        };
    }

    addNewSchoolToList() {
        if (this.sourceSchool) {
            Object.assign(this.sourceSchool, this.editedSchool);

            let sourceIndex = this.rowsCache.indexOf(this.sourceSchool, 0);
            if (sourceIndex > -1)
                Utilities.moveArrayItem(this.rowsCache, sourceIndex, 0);

            sourceIndex = this.rows.indexOf(this.sourceSchool, 0);
            if (sourceIndex > -1)
                Utilities.moveArrayItem(this.rows, sourceIndex, 0);

           this.editedSchool = null;
            this.sourceSchool = null;
        }
        else {
            let school = new School();
            Object.assign(school, this.editedSchool);
            this.editedSchool = null;

            let maxIndex = 0;
            for (let u of this.rowsCache) {
                if ((<any>u).index > maxIndex)
                    maxIndex = (<any>u).index;
            }

            (<any>school).index = maxIndex + 1;

            this.rowsCache.splice(0, 0, school);
            this.rows.splice(0, 0, school);
            this.rows = [...this.rows];

        }

        this.loadData();
    }


    loadData() {
        this.alertService.startLoadingMessage();
        this.loadingIndicator = true;
        this.setupService.getSchools().subscribe(school => this.onDataLoadSuccessful(school), error => this.onDataLoadFailed(error));

    }


    onDataLoadFailed(error: any) {
        this.alertService.stopLoadingMessage();
        this.loadingIndicator = false;

        this.alertService.showStickyMessage("Load Error", `Unable to retrieve users from the server.\r\nErrors: "${Utilities.getHttpResponseMessage(error)}"`,
            MessageSeverity.error, error);
    }


    onDataLoadSuccessful(school: School[]) {
        this.alertService.stopLoadingMessage();
        this.loadingIndicator = false;

        school.forEach((type, index, school) => {
            (<any>type).index = index + 1;
        });

        this.rowsCache = [...school];
        this.rows = school;

    }


    onSearchChanged(value: string) {
        this.rows = this.rowsCache.filter(r => Utilities.searchArray(value, false, r.schoolName, r.schoolValue));
    }


    onEditorModalHidden() {
       this.editingSchoolName = null;
        this.schoolEditor.resetForm(true);
    }


    newSchool() {
       this.editingSchoolName = null;
       this.sourceSchool = null;
        this.editedSchool = this.schoolEditor.newSchool();
        this.editorModal.show();
    }


    editSchool(row: School) {
        this.editingSchoolName = { name: row.schoolName };
        this.sourceSchool = row;
        this.editedSchool = this.schoolEditor.editSchool(row);
        this.editorModal.show();
    }

    deleteSchool(row: School) {

        this.alertService.showDialog('Are you sure you want to delete \"' + row.schoolName + '\"?', DialogType.confirm, () => this.deleteSchoolHelper(row));
    }


    deleteSchoolHelper(row: School) {

        this.alertService.startLoadingMessage("Deleting...");
        this.loadingIndicator = true;

        this.setupService.deleteSchool(row)
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
