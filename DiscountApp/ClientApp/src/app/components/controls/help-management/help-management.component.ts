import { Component, OnInit, AfterViewInit, TemplateRef, ViewChild, Input } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { Help } from '../../../models/help.model';
import { AlertService, DialogType, MessageSeverity } from '../../../services/alert.service';
import { AppTranslationService } from "../../../services/app-translation.service";
import { SetupService } from "../../../services/setup.service";
import { Utilities } from "../../../services/utilities";
import { HelpInfoComponent } from '../help-info/help-info.component';
@Component({
    selector: 'help-management',
    templateUrl: './help-management.component.html',
    styleUrls: ['./help-management.component.css']
})
export class HelpManagementComponent implements OnInit,AfterViewInit{

    loadingIndicator: boolean;
    columns: any[] = [];
    rows: Help[] = [];
    rowsCache: Help[] = [];
    editedHelp: Help;
    sourceHelp: Help;
    editingHelpName: { name: string };



    @ViewChild('indexTemplate')
    indexTemplate: TemplateRef<any>;

    @ViewChild('actionsTemplate')
    actionsTemplate: TemplateRef<any>;

    @ViewChild('helpEditor')
    helpEditor: HelpInfoComponent;

    @ViewChild('editorModal')
    editorModal: ModalDirective;



    constructor(private alertService: AlertService, private translationService: AppTranslationService, private setupService: SetupService) {
    }

    ngOnInit() {

        let gT = (key: string) => this.translationService.getTranslation(key);

      
        this.columns = [
            { prop: "index", name: '#', width: 50, cellTemplate: this.indexTemplate, canAutoResize: false },         
            { prop: 'helpName', name: gT('help.Name'), width: 200 },                  
            { name: '', width: 130, cellTemplate: this.actionsTemplate, resizeable: false, canAutoResize: false, sortable: false, draggable: false }
        ];
        
     

        this.loadData();
    }

    ngAfterViewInit() {

        this.helpEditor.changesSavedCallback = () => {
            this.addNewHelpToList();
            this.editorModal.hide();
        };

        this.helpEditor.changesCancelledCallback = () => {
            this.editedHelp = null;
            this.sourceHelp = null;
            this.editorModal.hide();
        };
    }

    addNewHelpToList() {
        if (this.sourceHelp) {
            Object.assign(this.sourceHelp, this.editedHelp);

            let sourceIndex = this.rowsCache.indexOf(this.sourceHelp, 0);
            if (sourceIndex > -1)
                Utilities.moveArrayItem(this.rowsCache, sourceIndex, 0);

            sourceIndex = this.rows.indexOf(this.sourceHelp, 0);
            if (sourceIndex > -1)
                Utilities.moveArrayItem(this.rows, sourceIndex, 0);

           this.editedHelp = null;
            this.sourceHelp = null;
        }
        else {
            let help = new Help();
            Object.assign(help, this.editedHelp);
            this.editedHelp = null;

            let maxIndex = 0;
            for (let u of this.rowsCache) {
                if ((<any>u).index > maxIndex)
                    maxIndex = (<any>u).index;
            }

            (<any>help).index = maxIndex + 1;

            this.rowsCache.splice(0, 0, help);
            this.rows.splice(0, 0, help);
            this.rows = [...this.rows];

        }

       this.loadData();
    }


    loadData() {
        this.alertService.startLoadingMessage();
        this.loadingIndicator = true;
        this.setupService.getHelps().subscribe(help => this.onDataLoadSuccessful(help), error => this.onDataLoadFailed(error));

    }


    onDataLoadFailed(error: any) {
        this.alertService.stopLoadingMessage();
        this.loadingIndicator = false;

        this.alertService.showStickyMessage("Load Error", `Unable to retrieve users from the server.\r\nErrors: "${Utilities.getHttpResponseMessage(error)}"`,
            MessageSeverity.error, error);
    }


    onDataLoadSuccessful(help: Help[]) {
        this.alertService.stopLoadingMessage();
        this.loadingIndicator = false;

        help.forEach((type, index, help) => {
            (<any>type).index = index + 1;
        });

        this.rowsCache = [...help];
        this.rows = help;

    }


    onSearchChanged(value: string) {
        this.rows = this.rowsCache.filter(r => Utilities.searchArray(value, false, r.helpName));
    }


    onEditorModalHidden() {
       this.editingHelpName = null;
        this.helpEditor.resetForm(true);
    }


    newHelp() {
       this.editingHelpName = null;
       this.sourceHelp = null;
        this.editedHelp = this.helpEditor.newHelp();
        this.editorModal.show();
    }


    editHelp(row: Help) {
        this.editingHelpName = { name: row.helpName };
        this.sourceHelp = row;
        this.editedHelp = this.helpEditor.editHelp(row);
        this.editorModal.show();
    }

    deleteHelp(row: Help) {

        this.alertService.showDialog('Are you sure you want to delete \"' + row.helpName + '\"?', DialogType.confirm, () => this.deleteHelpHelper(row));
    }


    deleteHelpHelper(row: Help) {

        this.alertService.startLoadingMessage("Deleting...");
        this.loadingIndicator = true;

        this.setupService.deleteHelp(row)
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
