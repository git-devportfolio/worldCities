import { Component, ViewChild } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { Country } from './country';
import { CountryService } from './countryService';

@Component({
    selector: 'app-countries',
    templateUrl: './countries.component.html',
    styleUrl: './countries.component.scss'
})
export class CountriesComponent {
    public displayedColumns: string[] = ['id', 'name', 'iso2', 'iso3', 'totCities'];
    public countries!: MatTableDataSource<Country>;

    defaultPageIndex: number = 0;
    defaultPageSize: number = 10;
    public defaultSortColumn: string = "name";
    public defaultSortOrder: "asc" | "desc" = "asc";

    defaultFilterColumn: string = "name";
    filterQuery?: string;

    @ViewChild(MatPaginator) paginator!: MatPaginator;
    @ViewChild(MatSort) sort!: MatSort;
    constructor(
        private countryService: CountryService) {
    }

    ngOnInit() {
        this.loadData();
    }

    loadData(query?: string) {
        var pageEvent = new PageEvent();
        pageEvent.pageIndex = 0;
        pageEvent.pageSize = 10;
        this.filterQuery = query;
        this.getData(pageEvent);
    }

    getData(event: PageEvent) {

        var sortColumn = (this.sort)
            ? this.sort.active
            : this.defaultSortColumn;
        var sortOrder = (this.sort)
            ? this.sort.direction
            : this.defaultSortOrder;
        var filterColumn = (this.filterQuery)
            ? this.defaultFilterColumn
            : null;
        var filterQuery = (this.filterQuery)
            ? this.filterQuery
            : null;

        this.countryService.getData(
            event.pageIndex,
            event.pageSize,
            sortColumn,
            sortOrder,
            filterColumn,
            filterQuery).subscribe({
                next: (result) => {
                    this.paginator.length = result.totalCount;
                    this.paginator.pageIndex = result.pageIndex;
                    this.paginator.pageSize = result.pageSize;
                    this.countries = new MatTableDataSource<Country>(result.data);
                },
                error: (error) => console.error(error)
            });
    }
}
