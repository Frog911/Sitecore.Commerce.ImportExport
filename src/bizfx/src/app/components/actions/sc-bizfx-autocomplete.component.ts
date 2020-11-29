import { Component, Input, OnInit, OnDestroy } from '@angular/core';
import { Observable, Subject, of } from 'rxjs';
import { ScBizFxView, ScBizFxProperty } from '@sitecore/bizfx';
import { ScBizFxSearchService, ScBizFxContextService } from '@sitecore/bizfx';
import { FormGroup } from '@angular/forms';

import { takeWhile, debounceTime, distinctUntilChanged, switchMap, catchError } from 'rxjs/operators';

/**
 * BizFx Autocomplete `Component`
 */
@Component({
    selector: 'sc-bizfx-autocomplete',
    styleUrls: ['./sc-bizfx-autocomplete.component.css'],
    templateUrl: './sc-bizfx-autocomplete.component.html'
})

export class ScBizFxAutocompleteComponent implements OnInit, OnDestroy {
    /**
    * Defines the property to render
    */
    @Input() property: ScBizFxProperty;
    /**
    * @ignore
    */
    selectedItem: any = {};
    @Input() actionForm: FormGroup;
    /**
    * @ignore
    */
    noResultsFound = false;
    /**
    * @ignore
    */
    private policyScope: any;
    /**
    * @ignore
    */
    private searchPolicy: any;
    /**
    * @ignore
    */
    private variantSearch = false;
    /**
     * @ignore
     */
    private alive = true;
    /**
     * @ignore
     */
    private searchTerms = new Subject<string>();
    /**
    * @ignore
    */
    private results: Observable<ScBizFxView>;
    /**
    * @ignore
    */
    searching = false;
    /**
    * @ignore
    */
    formattedResults: any[];

    constructor(
        private searchService: ScBizFxSearchService,
        private bizFxContext: ScBizFxContextService) {
    }

    /**
    * @ignore
    */
    ngOnInit(): void {
        const debounceTimeoutDefault = 300;
        let debounceTimeout = 0;

        const propertyPolicy = this.property.Policies.find(p => p.PolicyId === 'EntityType');
        this.policyScope = propertyPolicy && propertyPolicy.Models[0] ? propertyPolicy.Models[0].Name : '';
        const variantModel = propertyPolicy && propertyPolicy.Models[1] ? propertyPolicy.Models[1] : null;
        this.variantSearch = variantModel && variantModel.Name === 'SearchVariants';
        const searchScopePolicy = this.property.Policies
            .find(p => p['@odata.type'] === '#Sitecore.Commerce.Plugin.Search.SearchScopePolicy');
        this.searchPolicy = searchScopePolicy ? searchScopePolicy.Name : '';

        // get the autocomplete timeout so that a search doesnt happen after every key press - defaults to 300ms
        debounceTimeout = this.bizFxContext.config.AutoCompleteTimeout_ms ?
            this.bizFxContext.config.AutoCompleteTimeout_ms : debounceTimeoutDefault;
        if (debounceTimeout <= 0 || debounceTimeout > 1000) {
            debounceTimeout = debounceTimeoutDefault;
        }

        this.results = this.searchTerms
            .pipe(
                debounceTime(debounceTimeout),
                distinctUntilChanged(),
                switchMap(term => term !== undefined && term.length > 3
                    ? this.doSearch(term)
                    : of<ScBizFxView>()),
                catchError(error => {
                    this.searching = false;
                    this.noResultsFound = false;
                    return of<ScBizFxView>();
                })
            );

        this.results
            .pipe(takeWhile(() => this.alive))
            .subscribe(view => {
                this.searching = false;
                this.updateItems(view);
            });
    }

    /**
     * @ignore
     */
    ngOnDestroy(): void {
        this.alive = false;
    }

    /**
    * Executes the search
    */
    search(term: string) {
        this.searchTerms.next(term);
        this.formattedResults = [];
        this.actionForm.controls[this.property.Name].setValue(term);
    }

    doSearch(term: string): Observable<ScBizFxView> {
        this.searching = true;
        return this.searchService.searchTerm(term, this.searchPolicy);
    }

    /**
    * Handles the search results.
    */
    updateItems(view: ScBizFxView) {
        let searchResults;
        if (view !== undefined) {
            const children = view.ChildViews;
            if (this.policyScope === '') {
                searchResults = children;
            } else {
                searchResults = children.filter(c => c.EntityId.startsWith('Entity-' + this.policyScope + '-'));
            }

            this.formattedResults = [];

            for (const result of searchResults) {
                const properties = result['Properties'];
                const entityId = result['EntityId'];
                const displayName = properties.filter(p => p.Name === 'displayname')[0].Value;
                this.formattedResults.push({ 'title': displayName, 'id': entityId });

                if (this.variantSearch) {
                    const variantIdsProperty = properties.find(p => p.Name === 'variantid' && p.Value !== '');
                    const variantIds = variantIdsProperty && variantIdsProperty.Value ? variantIdsProperty.Value.split('|') : [];
                    const variantNamesProperty = properties.find(p => p.Name === 'variantdisplayname' && p.Value !== '');
                    const variantDisplayNames = variantNamesProperty && variantNamesProperty.Value
                        ? variantNamesProperty.Value.split('|') : [];

                    for (let i = 0; i < variantDisplayNames.length; i++) {
                        this.formattedResults
                            .push({ 'title': '  -> ' + variantDisplayNames[i], 'id': result['EntityId'] + '|' + variantIds[i] });
                    }
                }
            }

            this.noResultsFound = this.formattedResults.length === 0;
        } else {
            this.noResultsFound = true;
        }
    }

    /**
    * Handles when a search result is selected.
    */
    selected(item: any) {
        this.selectedItem = item;
        this.formattedResults = [];
        this.actionForm.controls[this.property.Name].setValue(item.id);
    }
}
