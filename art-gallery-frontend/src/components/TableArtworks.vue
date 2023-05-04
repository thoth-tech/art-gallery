<template>
    <div class="main-div">
        <div class="search-icon">
            <span class="screen-reader-only">Search</span>
            <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512">
                <!--! Font Awesome Pro 6.2.1 by @fontawesome - https://fontawesome.com License - https://fontawesome.com/license (Commercial License) Copyright 2022 Fonticons, Inc. -->
                <path d="M416 208c0 45.9-14.9 88.3-40 122.7L502.6 457.4c12.5 12.5 12.5 32.8 0 45.3s-32.8 12.5-45.3 0L330.7 376c-34.4 25.2-76.8 40-122.7 40C93.1 416 0 322.9 0 208S93.1 0 208 0S416 93.1 416 208zM208 352c79.5 0 144-64.5 144-144s-64.5-144-144-144S64 128.5 64 208s64.5 144 144 144z"/>
            </svg>
        </div>
        <input type="text" class="search-input" v-on:keyup="filterTable()" placeholder="Search..."/>
        <div class="table-div">
            <table class="table-art" id="table1" cellpadding="8px" border="1">
                <thead>
                    <tr>
                        <th> </th>
                        <th>TITLE</th>
                        <th>DESCRIPTION</th>
                        <th>ARTISTS</th>
                        <th>MEDIA</th>
                        <th>YEAR</th>
                    </tr>
                </thead>
            </table>
        </div>
    </div>
</template>

<script>
    import { getArtworks } from '../services/ArtworkService'

    export default {
    name: "TableArtworks",
    data() {
        return {
            artworks: [],
            contributingArtists: ""
        }
    },
    methods: {
        // Gets the data from endpoint and stores in an array.
        async fetchArtworks() {
            await getArtworks()
                .then(data => {
                    this.artworks = data;
            });
            this.getContributingArtists();
            this.fillTableWithData();
        },
        // Adds space between artist names.
        getContributingArtists() {
            this.artworks.forEach( item => {
                item.contributingArtists = item.contributingArtists.join(", ")
            })
        },
        // Fill table with data from artwork service.
        fillTableWithData() {
            const table = document.getElementById("table1");

            this.artworks.forEach((item) => {
                var row = table.insertRow(-1);

                var cell = row.insertCell(-1);
                cell.innerHTML = "<img src=" + item.primaryImageUrl + " width=200px/>";
                cell.style = "text-align: center; width: 200px; padding: 15px;"

                cell = row.insertCell(-1);
                cell.innerHTML = item.title;

                cell = row.insertCell(-1);
                cell.innerHTML =  item.description;
                cell.style = "text-align: justify; font-size: 13px;"

                cell = row.insertCell(-1);
                cell.innerHTML = item.contributingArtists;

                cell = row.insertCell(-1);
                cell.innerHTML = item.mediaType;

                cell = row.insertCell(-1);
                cell.innerHTML = item.yearCreated;
            });
            table.classList.add("table-data");
            this.tableHover();
        },
        // Handles hover effect on table.
        tableHover() {
            var table = document.getElementById("table1");
            var rows = table.getElementsByTagName("tr");

            for (var i = 1; i < rows.length; i++) {
                var currentRow = table.rows[i];

                var highlightRow = function(row) {
                    return function() {
                        row.style = "background-color:var(--color--grey-light)";
                    };
                };
                var removeHighlight = function(row) {
                    return function() {
                        row.style = "background-color:var(--color--white)";
                    }
                }
                currentRow.onmouseover = highlightRow(currentRow);
                currentRow.onmouseleave = removeHighlight(currentRow);
            }
        },
        // Filter table entries.
        filterTable() {
            var tr, td, input, filter, index, textValue, table;
            table = document.getElementById("table1");
            tr = table.getElementsByTagName("tr");
            input = document.querySelector(".search-input");
            filter = input.value.toUpperCase();

            for (index = 0; index < tr.length; index++) {
                td = tr[index].getElementsByTagName("td")[1];
                if (td) {
                    textValue = td.innerText;
                    if (textValue.toUpperCase().indexOf(filter) > -1) {
                        tr[index].style.display = "";
                    }
                    else {
                        tr[index].style.display = "none";
                    }
                }
            }
        }
    },
    mounted() {
        this.fetchArtworks();
    }
};
</script>

<style scoped>
    .main-div {
        margin-top: 50px;
        margin-bottom: 20px;
        margin-left: auto;
        margin-right: auto;
        text-align: center;
    }

    .table-div {
        margin-top: 10px;
        margin-left: auto;
        margin-right: auto;
        width: 100%;
        overflow-x:auto;
    }

    .table-art {
        margin-left: auto;
        margin-right: auto;
        width: 100%;
        border-collapse: collapse;
    }

    .table-art th {
        color:var(--color--charcoal);
        background-color: var(--color--primary-hover-light);
        padding-top: 20px;
        padding-bottom: 20px;
    }

    .table-data {
        color:var(--color--charcoal);
        padding-top: 20px;
        padding-bottom: 20px;
        font-family:var(--font--base);
        font-size: 18px;
        border: 1px solid var(--color--grey);
    }

    .search-input {
        width: 50%;
        max-width: 800px;
        padding: 10px;
        border: var(--color--grey-med) solid 1px;
        margin-bottom: 20px;
        font-size: 16px;
        vertical-align: top;
    }

    .search-input::placeholder {
        font-size: 16px;
    }

    .search-icon {
        margin-right: 15px;
        display: inline;
        vertical-align: top;
    }

    .search-icon svg {
        padding-top: 5px;
        fill: var(--color--black);
        max-width: 28px;
        width: calc(24px + 6 * (100vw - 320px) / 1040);
    }

    @media only screen and (max-width: 600px) {
        .img-exh {
            padding-top: 1px;
            padding-bottom: 1px;
            padding-left: 1px;
            padding-right: 1px;
            max-height: 120px;
        }

        .main-div {
            margin: 0px;
        }

        .table-art th {
            padding: 2px;
            font-size: 16px;
        }

        .table-data {
            font-size: 14px;
            padding-left: 2px;
            padding-right: 2px;
            overflow: auto;
        }

        .h3-title {
            font-size: 14px;
        }
    }

</style>
