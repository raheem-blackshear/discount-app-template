import { Component, OnInit, AfterViewInit, TemplateRef, ViewChild, Input } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { Nationality } from '../../../models/nationality.model';
import { AlertService, DialogType, MessageSeverity } from '../../../services/alert.service';
import { AppTranslationService } from "../../../services/app-translation.service";
import { SetupService } from "../../../services/setup.service";
import { Utilities } from "../../../services/utilities";
import { NationalityInfoComponent } from '../nationality-info/nationality-info.component';
@Component({
    selector: 'nationality-management',
    templateUrl: './nationality-management.component.html',
    styleUrls: ['./nationality-management.component.css']
})
export class NationalityManagementComponent implements OnInit, AfterViewInit {

    loadingIndicator: boolean;
    columns: any[] = [];
    rows: Nationality[] = [];
    rowsCache: Nationality[] = [];
    editedNationality: Nationality;
    sourceNationality: Nationality;
    editingNationalityName: { name: string };



    @ViewChild('indexTemplate')
    indexTemplate: TemplateRef<any>;

    @ViewChild('actionsTemplate')
    actionsTemplate: TemplateRef<any>;

    @ViewChild('nationalityEditor')
    nationalityEditor: NationalityInfoComponent;

    @ViewChild('editorModal')
    editorModal: ModalDirective;



    constructor(private alertService: AlertService, private translationService: AppTranslationService, private setupService: SetupService) {
    }

    ngOnInit() {

        let gT = (key: string) => this.translationService.getTranslation(key);

      
        this.columns = [
            { prop: "index", name: '#', width: 50, cellTemplate: this.indexTemplate, canAutoResize: false },         
            { prop: 'nationalityName', name: gT('nationality.Name'), width: 200 },                  
            { name: '', width: 130, cellTemplate: this.actionsTemplate, resizeable: false, canAutoResize: false, sortable: false, draggable: false }
        ];
        
     

        this.loadData();
    }

    ngAfterViewInit() {

        this.nationalityEditor.changesSavedCallback = () => {
            this.addNewNationalityToList();
            this.editorModal.hide();
        };

        this.nationalityEditor.changesCancelledCallback = () => {
            this.editedNationality = null;
            this.sourceNationality = null;
            this.editorModal.hide();
        };
    }

    addNewNationalityToList() {
        if (this.sourceNationality) {
            Object.assign(this.sourceNationality, this.editedNationality);

            let sourceIndex = this.rowsCache.indexOf(this.sourceNationality, 0);
            if (sourceIndex > -1)
                Utilities.moveArrayItem(this.rowsCache, sourceIndex, 0);

            sourceIndex = this.rows.indexOf(this.sourceNationality, 0);
            if (sourceIndex > -1)
                Utilities.moveArrayItem(this.rows, sourceIndex, 0);

           this.editedNationality = null;
            this.sourceNationality = null;
        }
        else {
            let nationality = new Nationality();
            Object.assign(nationality, this.editedNationality);
            this.editedNationality = null;

            let maxIndex = 0;
            for (let u of this.rowsCache) {
                if ((<any>u).index > maxIndex)
                    maxIndex = (<any>u).index;
            }

            (<any>nationality).index = maxIndex + 1;

            this.rowsCache.splice(0, 0, nationality);
            this.rows.splice(0, 0, nationality);
            this.rows = [...this.rows];

        }

        this.loadData();
    }


    loadData() {
        this.alertService.startLoadingMessage();
        this.loadingIndicator = true;
        this.setupService.getNationalities().subscribe(nationality => this.onDataLoadSuccessful(nationality), error => this.onDataLoadFailed(error));

    }


    onDataLoadFailed(error: any) {
        this.alertService.stopLoadingMessage();
        this.loadingIndicator = false;

        this.alertService.showStickyMessage("Load Error", `Unable to retrieve users from the server.\r\nErrors: "${Utilities.getHttpResponseMessage(error)}"`,
            MessageSeverity.error, error);
    }


    onDataLoadSuccessful(nationality: Nationality[]) {
        this.alertService.stopLoadingMessage();
        this.loadingIndicator = false;

        nationality.forEach((type, index, nationality) => {
            (<any>type).index = index + 1;
        });

        this.rowsCache = [...nationality];
        this.rows = nationality;

    }


    onSearchChanged(value: string) {
        this.rows = this.rowsCache.filter(r => Utilities.searchArray(value, false, r.nationalityName));
    }


    onEditorModalHidden() {
       this.editingNationalityName = null;
        this.nationalityEditor.resetForm(true);
    }


    newNationality() {
       this.editingNationalityName = null;
       this.sourceNationality = null;
        this.editedNationality = this.nationalityEditor.newNationality();
        this.editorModal.show();
    }


    editNationality(row: Nationality) {
        this.editingNationalityName = { name: row.nationalityName };
        this.sourceNationality = row;
        this.editedNationality = this.nationalityEditor.editNationality(row);
        this.editorModal.show();
    }

    deleteNationality(row: Nationality) {

        this.alertService.showDialog('Are you sure you want to delete \"' + row.nationalityName + '\"?', DialogType.confirm, () => this.deleteNationalityHelper(row));
    }


    deleteNationalityHelper(row: Nationality) {

        this.alertService.startLoadingMessage("Deleting...");
        this.loadingIndicator = true;

        this.setupService.deleteNationality(row)
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
