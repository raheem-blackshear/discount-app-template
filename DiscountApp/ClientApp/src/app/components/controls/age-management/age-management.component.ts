import { Component, OnInit, AfterViewInit, TemplateRef, ViewChild, Input } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { Age } from '../../../models/age.model';
import { AlertService, DialogType, MessageSeverity } from '../../../services/alert.service';
import { AppTranslationService } from "../../../services/app-translation.service";
import { SetupService } from "../../../services/setup.service";
import { Utilities } from "../../../services/utilities";
import { AgeInfoComponent } from '../age-info/age-info.component';
@Component({
    selector: 'age-management',
    templateUrl: './age-management.component.html',
    styleUrls: ['./age-management.component.css']
})
export class AgeManagementComponent implements OnInit,AfterViewInit{

    loadingIndicator: boolean;
    columns: any[] = [];
    rows: Age[] = [];
    rowsCache: Age[] = [];
    editedAge: Age;
    sourceAge: Age;
    editingAgeName: { name: string };



    @ViewChild('indexTemplate')
    indexTemplate: TemplateRef<any>;

    @ViewChild('actionsTemplate')
    actionsTemplate: TemplateRef<any>;

    @ViewChild('ageEditor')
    ageEditor: AgeInfoComponent;

    @ViewChild('editorModal')
    editorModal: ModalDirective;



    constructor(private alertService: AlertService, private translationService: AppTranslationService, private setupService: SetupService) {
    }

    ngOnInit() {

        let gT = (key: string) => this.translationService.getTranslation(key);

      
        this.columns = [
            { prop: "index", name: '#', width: 50, cellTemplate: this.indexTemplate, canAutoResize: false },         
            { prop: 'ageName', name: gT('age.Name'), width: 200 },                  
            { name: '', width: 130, cellTemplate: this.actionsTemplate, resizeable: false, canAutoResize: false, sortable: false, draggable: false }
        ];
        
     

        this.loadData();
    }

    ngAfterViewInit() {

        this.ageEditor.changesSavedCallback = () => {
            this.addNewAgeToList();
            this.editorModal.hide();
        };

        this.ageEditor.changesCancelledCallback = () => {
            this.editedAge = null;
            this.sourceAge = null;
            this.editorModal.hide();
        };
    }

    addNewAgeToList() {
        if (this.sourceAge) {
            Object.assign(this.sourceAge, this.editedAge);

            let sourceIndex = this.rowsCache.indexOf(this.sourceAge, 0);
            if (sourceIndex > -1)
                Utilities.moveArrayItem(this.rowsCache, sourceIndex, 0);

            sourceIndex = this.rows.indexOf(this.sourceAge, 0);
            if (sourceIndex > -1)
                Utilities.moveArrayItem(this.rows, sourceIndex, 0);

           this.editedAge = null;
            this.sourceAge = null;
        }
        else {
            let age = new Age();
            Object.assign(age, this.editedAge);
            this.editedAge = null;

            let maxIndex = 0;
            for (let u of this.rowsCache) {
                if ((<any>u).index > maxIndex)
                    maxIndex = (<any>u).index;
            }

            (<any>age).index = maxIndex + 1;

            this.rowsCache.splice(0, 0, age);
            this.rows.splice(0, 0, age);
            this.rows = [...this.rows];

        }

       this.loadData();
    }


    loadData() {
        this.alertService.startLoadingMessage();
        this.loadingIndicator = true;
        this.setupService.getAges().subscribe(age => this.onDataLoadSuccessful(age), error => this.onDataLoadFailed(error));

    }


    onDataLoadFailed(error: any) {
        this.alertService.stopLoadingMessage();
        this.loadingIndicator = false;

        this.alertService.showStickyMessage("Load Error", `Unable to retrieve users from the server.\r\nErrors: "${Utilities.getHttpResponseMessage(error)}"`,
            MessageSeverity.error, error);
    }


    onDataLoadSuccessful(age: Age[]) {
        this.alertService.stopLoadingMessage();
        this.loadingIndicator = false;

        age.forEach((type, index, age) => {
            (<any>type).index = index + 1;
        });

        this.rowsCache = [...age];
        this.rows = age;

    }


    onSearchChanged(value: string) {
        this.rows = this.rowsCache.filter(r => Utilities.searchArray(value, false, r.ageName));
    }


    onEditorModalHidden() {
       this.editingAgeName = null;
        this.ageEditor.resetForm(true);
    }


    newAge() {
       this.editingAgeName = null;
       this.sourceAge = null;
        this.editedAge = this.ageEditor.newAge();
        this.editorModal.show();
    }


    editAge(row: Age) {
        this.editingAgeName = { name: row.ageName };
        this.sourceAge = row;
        this.editedAge = this.ageEditor.editAge(row);
        this.editorModal.show();
    }

    deleteAge(row: Age) {

        this.alertService.showDialog('Are you sure you want to delete \"' + row.ageName + '\"?', DialogType.confirm, () => this.deleteAgeHelper(row));
    }


    deleteAgeHelper(row: Age) {

        this.alertService.startLoadingMessage("Deleting...");
        this.loadingIndicator = true;

        this.setupService.deleteAge(row)
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
