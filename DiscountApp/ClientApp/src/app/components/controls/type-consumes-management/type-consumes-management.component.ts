import { Component, OnInit, AfterViewInit, TemplateRef, ViewChild, Input } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { TypeConsume } from '../../../models/typeConsume.model';
import { AlertService, DialogType, MessageSeverity } from '../../../services/alert.service';
import { AppTranslationService } from "../../../services/app-translation.service";
import { SetupService } from "../../../services/setup.service";
import { Utilities } from "../../../services/utilities";
import { TypeConsumesInfoComponent } from '../type-consumes-info/type-consumes-info.component';
@Component({
    selector: 'type-Consumes-management',
    templateUrl: './type-consumes-management.component.html',
    styleUrls: ['./type-consumes-management.component.css']
})
export class TypeConsumesManagementComponent implements OnInit, AfterViewInit {

    loadingIndicator: boolean;
    columns: any[] = [];
    rows: TypeConsume[] = [];
    rowsCache: TypeConsume[] = [];
    editedTypeConsume: TypeConsume;
    sourceTypeConsume: TypeConsume;
    editingTypeConsumeName: { name: string };



    @ViewChild('indexTemplate')
    indexTemplate: TemplateRef<any>;

    @ViewChild('actionsTemplate')
    actionsTemplate: TemplateRef<any>;

    @ViewChild('typeConsumeEditor')
    typeConsumeEditor: TypeConsumesInfoComponent;

    @ViewChild('editorModal')
    editorModal: ModalDirective;



    constructor(private alertService: AlertService, private translationService: AppTranslationService, private setupService: SetupService) {
    }

    ngOnInit() {

        let gT = (key: string) => this.translationService.getTranslation(key);


        this.columns = [
            { prop: "index", name: '#', width: 50, cellTemplate: this.indexTemplate, canAutoResize: false },
            { prop: 'typeConsumeName', name: gT('typeConsume.Name'), width: 200 },
            { prop: 'typeConsumeValue', name: gT('typeConsume.Value'), width: 350 },
            { name: '', width: 130, cellTemplate: this.actionsTemplate, resizeable: false, canAutoResize: false, sortable: false, draggable: false }
        ];



        this.loadData();
    }

    ngAfterViewInit() {

        this.typeConsumeEditor.changesSavedCallback = () => {
            this.addNewTypeConsumeToList();
            this.editorModal.hide();
        };

        this.typeConsumeEditor.changesCancelledCallback = () => {
            this.editedTypeConsume = null;
            this.sourceTypeConsume = null;
            this.editorModal.hide();
        };
    }

    addNewTypeConsumeToList() {
        if (this.sourceTypeConsume) {
            Object.assign(this.sourceTypeConsume, this.editedTypeConsume);

            let sourceIndex = this.rowsCache.indexOf(this.sourceTypeConsume, 0);
            if (sourceIndex > -1)
                Utilities.moveArrayItem(this.rowsCache, sourceIndex, 0);

            sourceIndex = this.rows.indexOf(this.sourceTypeConsume, 0);
            if (sourceIndex > -1)
                Utilities.moveArrayItem(this.rows, sourceIndex, 0);

            this.editedTypeConsume = null;
            this.sourceTypeConsume = null;
        }
        else {
            let typeConsume = new TypeConsume();
            Object.assign(typeConsume, this.editedTypeConsume);
            this.editedTypeConsume = null;

            let maxIndex = 0;
            for (let u of this.rowsCache) {
                if ((<any>u).index > maxIndex)
                    maxIndex = (<any>u).index;
            }

            (<any>typeConsume).index = maxIndex + 1;

            this.rowsCache.splice(0, 0, typeConsume);
            this.rows.splice(0, 0, typeConsume);
            this.rows = [...this.rows];

        }

        // this.loadData();
    }


    loadData() {
        this.alertService.startLoadingMessage();
        this.loadingIndicator = true;
        this.setupService.getTypeConsumes().subscribe(typeConsumes => this.onDataLoadSuccessful(typeConsumes), error => this.onDataLoadFailed(error));

    }


    onDataLoadFailed(error: any) {
        this.alertService.stopLoadingMessage();
        this.loadingIndicator = false;

        this.alertService.showStickyMessage("Load Error", `Unable to retrieve users from the server.\r\nErrors: "${Utilities.getHttpResponseMessage(error)}"`,
            MessageSeverity.error, error);
    }


    onDataLoadSuccessful(typeConsumes: TypeConsume[]) {
        this.alertService.stopLoadingMessage();
        this.loadingIndicator = false;

        typeConsumes.forEach((type, index, typeConsumes) => {
            (<any>type).index = index + 1;
        });

        this.rowsCache = [...typeConsumes];
        this.rows = typeConsumes;

    }


    onSearchChanged(value: string) {
        this.rows = this.rowsCache.filter(r => Utilities.searchArray(value, false, r.typeConsumeName, r.typeConsumeValue));
    }


    onEditorModalHidden() {
        this.editingTypeConsumeName = null;
        this.typeConsumeEditor.resetForm(true);
    }


    newTypeConsume() {
        this.editingTypeConsumeName = null;
        this.sourceTypeConsume = null;
        this.editedTypeConsume = this.typeConsumeEditor.newTypeConsume();
        this.editorModal.show();
    }


    editTypeConsume(row: TypeConsume) {
        this.editingTypeConsumeName = { name: row.typeConsumeName };
        this.sourceTypeConsume = row;
        this.editedTypeConsume = this.typeConsumeEditor.editTypeConsume(row);
        this.editorModal.show();           
       
    }

    deleteTypeConsume(row: TypeConsume) {

        this.alertService.showDialog('Are you sure you want to delete \"' + row.typeConsumeName + '\"?', DialogType.confirm, () => this.deleteTypeConsumeHelper(row));
    }


    deleteTypeConsumeHelper(row: TypeConsume) {

        this.alertService.startLoadingMessage("Deleting...");
        this.loadingIndicator = true;

        this.setupService.deleteTypeConsume(row)
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
