<template>
    <div class="main-div">
        <div class="search-icon">
            <span class="screen-reader-only">Search</span>
            <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512">
                <!--! Font Awesome Pro 6.2.1 by @fontawesome - https://fontawesome.com License - https://fontawesome.com/license (Commercial License) Copyright 2022 Fonticons, Inc. -->
                <path d="M416 208c0 45.9-14.9 88.3-40 122.7L502.6 457.4c12.5 12.5 12.5 32.8 0 45.3s-32.8 12.5-45.3 0L330.7 376c-34.4 25.2-76.8 40-122.7 40C93.1 416 0 322.9 0 208S93.1 0 208 0S416 93.1 416 208zM208 352c79.5 0 144-64.5 144-144s-64.5-144-144-144S64 128.5 64 208s64.5 144 144 144z"/>
            </svg>
        </div>
        <input type="text" class="search-input" v-model="search" placeholder="Search..."/>
        <div class="table-div">
            <table class="table-exh" border="1">
                <thead>
                    <tr>
                        <th> </th>
                        <th>TITLE</th>
                        <th>DESCRIPTION</th>
                        <th>START</th>
                        <th>END</th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="item in filterTable()" :key="item">
                        <td><img :src="item.backgroundImageUrl" class="img-exh"/></td>
                        <td class="description">{{ item.name }}</td>
                        <td class="description">{{ item.description }}</td>
                        <td class="description">{{ item.startDate }}</td>
                        <td class="description">{{ item.endDate }}</td>
                    </tr>
                </tbody>
            </table>
            <button v-on:click="showInputs" class="plus-button" v-if="isAdmin()">+</button>
        </div>

        <div class="post-message" v-if="resultReceived && addExhibitionClicked" >
            <div v-if="success">
                <h3 class="success-text">{{ this.postResult }}</h3>
            </div>
            <div v-else>
                <h3 class="error-text">{{ this.postResult }}</h3>
            </div>
        </div>

        <div class="entry-div" v-if="addExhibitionClicked">
            <h2>Add New Exhibition</h2>
            <input type="text" v-model="name" class="table-input" placeholder="Enter name..."/>
            <input type="text" v-model="description" class="table-input" placeholder="Enter description..."/>
            <input type="text" v-model="backgroundImageUrl" class="table-input" placeholder="Enter backgroundImageUrl..."/>
            <!-- Added to match the new model - might be better as a special date input? -->
            <input type="text" v-model="startDate" class="table-input" placeholder="Enter start date as DD/MM/YYYY..."/>
            <input type="text" v-model="endDate" class="table-input" placeholder="Enter end date as DD/MM/YYYY..."/>
            <button v-on:click="addExhibition" class="add-exhibition-button">Add exhibition</button>
        </div>

    </div>
</template>

<script>
import { postExhibition } from '@/services/ExhibitionService';
import { mapState } from 'vuex';

export default {
    name: 'TableExhibition',
    props: {
        exhibitions: {
            type: Array,
        },
        fields: {
            type: Array,
        },
    },
    data() {
        return {
            search: "",
            addExhibitionClicked: false,
            name: "",
            description: "",
            backgroundImageUrl: "",
            startDate: "",
            endDate: "",
            postResult: "",
            resultReceived: false,
            success: false
        }
    },
    methods: {
        isAdmin() {
            if (this.account.user) {
                return this.account.user.role == "Admin";
            }
        },
        // Filter table.
        filterTable() {
            return this.exhibitions.filter(d => {
                return d.name.toUpperCase().indexOf(this.search.toUpperCase()) != -1;
            })
        },
        // Show the add entry inputs.
        showInputs() {
            if (!this.addExhibitionClicked)
                this.addExhibitionClicked = true;
            else
                this.addExhibitionClicked = false;
        },
        // Add entry to database.
        async addExhibition() {
            // Check for authentication credentials - doesn't check if the user is an admin, just that there is a user
            if (!this.account.user)
            {
                console.log('Error: not logged in')
            }
            if (this.account.user)
            {
                this.postResult = await postExhibition(this.name, this.description, this.backgroundImageUrl, this.startDate, this.endDate);

                if (this.postResult.name == this.name) {
                    this.postResult = "Your upload has been successful."
                    this.success = true
                }
                else 
                    this.success = false
 
                this.resultReceived = true;
            }
            if (this.exhibitions.name != "undefined")
            {
                this.$emit('updateData');
            }
        },
    },
    computed: {
        ...mapState({
            account: state => state.account
        })
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

    .table-exh {
        margin-top: 10px;
        width:100%;
    }

    .table-exh th {
        color: #343737;
        padding: 20px;
        background-color:#aad6c7;
        font-family:'Trebuchet MS', 'Lucida Sans Unicode', 'Lucida Grande', 'Lucida Sans', Arial, sans-serif;
        font-size: 20px;
    }

    .table-exh td, th {
        border: 1px solid #dedcdc;
    }

    .table-exh tr {
        background-color:#ffffff;
        border: 1px;
    }

    .table-exh tr:hover {
        background-color:#f5f5f5;
        border: 1px;
    }

    .table-exh td {
        color:var(--color--charcoal);
        padding-top: 20px;
        padding-bottom: 20px;
        font-family:var(--font--base);
        font-size: 16px;
        border: 1px solid var(--color--grey);
    }

    .search-input {
        width: 50%;
        max-width: 800px;
        padding: 10px;
        border: 1px solid #565b5c;
        margin-bottom: 40px;
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
        fill: black;
        max-width: 28px;
        width: calc(24px + 6 * (100vw - 320px) / 1040);
    }

    .plus-button {
        font-size: 25px;
        padding-left: 12px;
        padding-right: 12px;
        float:right;
        margin-top: 10px;
        margin-bottom: 20px;
    }

    .add-exhibition-button {
        color: var(--color--white);
        border: none;
        background-color: var(--color--primary);
        padding: 10px 20px;
        margin: 10px 15px;
        font-weight: var(--font--semibold);
        border-radius: 5px;
    }

    .table-input {
        display: block;
        padding: 10px;
        font-size: 16px;
        margin-right: 10px;
        margin-bottom: 15px;
        border: 0.5px solid rgb(146, 146, 146);
        border-radius: 3px;
        width: 100%;
    }

    .entry-div {
        display:block;
        background-color: rgb(245, 245, 245);
        box-shadow: 5px 10px rgb(227, 226, 226);
        border-radius: 10px;
        padding:30px;
        margin-left: auto;
        margin-right:auto;
        margin-bottom: 20px;
        max-width: 500px;
        width: 100%;
    }

    .table-div {
        margin-left: auto;
        margin-right: auto;
        width: 100%;
        overflow-x:auto;
    }

    .img-exh {
        width: 100%;
        max-width:350px;
        min-width:200px;
        max-height: 200px;
        object-fit: cover;
        display: block;
        margin-left: auto;
        margin-right: auto;
        padding-top: 10px;
        padding-bottom: 10px;
        padding-left: 20px;
        padding-right: 20px;
    }

    .description {
        max-width: 600px;
    }

    .success-text {
        color:green;
    }

    .error-text {
        color:red;
    }

    .post-message {
        margin-bottom:30px;
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

        .table-exh th {
            padding: 2px;
            font-size: 16px;
        }

        .table-exh td {
            font-size: 14px;
            padding-left: 2px;
            padding-right: 2px;
        }
    }

</style>
